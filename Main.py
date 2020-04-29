import numpy as np
import matplotlib.pyplot as plt
import os
import cv2
import random
import pickle

DATADIR = "C:/Datasets/SatelliteImages/"
CATAGORIES = ["CityBuilding", "DenseForest", "Grass", "Road", "Sand", "SparseForest", "VillageBuilding", "Water"]


# Resize all images to the same size
IMG_SIZE = 50
# create training data
training_data = []


def create_training_data():
    for category2 in CATAGORIES:
        # for each catagory file in our path
        currentImageCount = 0
        path2 = os.path.join(DATADIR, category2)
        class_num = CATAGORIES.index(category2)
        for img2 in os.listdir(path2):
            try:
                if currentImageCount < 600:
                    img_array2 = cv2.imread(os.path.join(path2, img2), cv2.IMREAD_COLOR)
                    new_array2 = cv2.resize(img_array2, (IMG_SIZE, IMG_SIZE))
                    training_data.append([new_array2, class_num])
                    currentImageCount = currentImageCount + 1
            except Exception as e:
                pass
        currentImageCount = 0


create_training_data()

random.shuffle(training_data)

X = []
Y = []

for features, label in training_data:
    X.append(features)
    Y.append(label)
X = np.array(X).reshape(-1, IMG_SIZE, IMG_SIZE, 3)

# Save our data
pickle_out = open("X.pickle", "wb")
pickle.dump(X, pickle_out)
pickle_out.close()

pickle_out = open("Y.pickle", "wb")
pickle.dump(Y, pickle_out)
pickle_out.close()

print("Finished")