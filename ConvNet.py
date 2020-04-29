import tensorflow as tf
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense, Dropout, Activation, Flatten, Conv2D, MaxPooling2D
from tensorflow.keras.callbacks import TensorBoard
import pickle
import time
import tensorflow.keras


NAME = "Satellite-Image-Classifier-cnn-64x2-{}".format(int(time.time()))

# Create callback board
tensorboard = TensorBoard(log_dir='logs\\{}'.format(NAME))

config = tf.ConfigProto()
config.gpu_options.allow_growth = True
session = tf.Session(config=config)


pickle_in = open("X.pickle", "rb")
X = pickle.load(pickle_in)

pickle_in = open("Y.pickle", "rb")
Y = pickle.load(pickle_in)
Y = tf.keras.utils.to_categorical(Y, 8)

#X = X/255.0


# dense_layers = [1, 2]
# layer_sizes = [32, 64, 128, 256]
# conv_layers = [1, 2, 3]
# kernal_sizes = [3, 7]
# denselayer_sizes = [8]
# num_epochs = [50, 100, 200, 300, 400, 500, 800, 1000, 1200]

dense_layers = [1]
dense_layer_sizes = [256]
conv_layers = [2]
conv_layer_sizes = [128]
kernal_sizes = [7]
num_epochs = [550]

for dense_layer in dense_layers:
    for dense_layer_size in dense_layer_sizes:
        for conv_layer in conv_layers:
            for conv_layer_size in conv_layer_sizes:
                for kernal_size in kernal_sizes:
                    for num_epoch in num_epochs:
                        NAME = "{}-conv-{}-nodes-{}-dense-{}-kernal-{}-numepoch-{}".format(conv_layer, dense_layer_size, dense_layer, kernal_size, num_epoch, int(time.time()))
                        print(NAME)

                        model = Sequential()

                        model.add(Conv2D(conv_layer_size, (kernal_size, kernal_size), input_shape=X.shape[1:]))
                        model.add(Activation('relu'))
                        model.add(MaxPooling2D(pool_size=(2, 2)))
                        model.add(Dropout(0.15))

                        for l in range(conv_layer-1):
                            model.add(Conv2D(conv_layer_size, (kernal_size, kernal_size)))
                            model.add(Activation('relu'))
                            model.add(MaxPooling2D(pool_size=(2, 2)))
                            model.add(Dropout(0.15))

                        model.add(Flatten())

                        for _ in range(dense_layer):
                            model.add(Dense(dense_layer_size))
                            model.add(Activation('relu'))
                            model.add(Dropout(0.35))

                        model.add(Dense(8))
                        model.add(Activation('softmax'))

                        tensorboard = TensorBoard(log_dir="logs\\{}".format(NAME))

                        model.compile(loss='categorical_crossentropy',
                                      optimizer='adadelta',
                                      metrics=['accuracy'])

                        model.fit(X, Y,
                                  batch_size=32,
                                  # steps_per_epoch=50,
                                  # validation_steps=25,
                                  epochs=num_epoch,
                                  validation_split=0.3,
                                  callbacks=[tensorboard],
                                  shuffle=True)

model.save('CNN.model')
