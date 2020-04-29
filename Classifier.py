import cv2
import tensorflow as tf
import os
import numpy as np
import json

CATAGORIES = ["CityBuilding", "DenseForest", "Grass", "Road", "Sand", "SparseForest", "VillageBuilding", "Water"]

def getCatagories():
    directory_list = list()
    for root, dirs, files in os.walk("C:\Datasets\SatelliteImages", topdown=False):
        for name in dirs:
            directory_list.append(name)

    catagories = np.asarray(directory_list)
    return catagories

def prepare(filepath):
    IMG_SIZE = 50
    img_array = cv2.imread(filepath, cv2.IMREAD_COLOR)
    new_array = cv2.resize(img_array, (IMG_SIZE, IMG_SIZE))
    return new_array.reshape(-1, IMG_SIZE, IMG_SIZE, 3)


def identify():
    model = tf.keras.models.load_model("CNN.model")
    prediction = model.predict([prepare('0-0.jpg')])

    print(prediction)

    return prediction


def identifySegment():
    model = tf.keras.models.load_model("CNN.model")

    pointsFound = []

    for img2 in os.listdir("Test"):
        prediction = model.predict([prepare("Test/" + img2)])
        currentMax = 0
        currentMaxVal = 0
        currentIndex = 0


        for pred in prediction[0]:
            if pred > currentMaxVal:
                currentMax = currentIndex
                currentMaxVal = pred
                currentIndex = currentIndex + 1
            # print(prediction)
            # print(currentMaxVal)

            result = CATAGORIES[currentMax]

        #bias removal for water being mistook as trees due to large amount of noise and dark colours in both
        if(prediction[0][7] > 0.1):
            currentMax = 7
            currentMaxVal = prediction[0][7]

        #bias removal for sand having less images
        if(prediction[0][4] > 0.1):
            currentMax = 5
            currentMaxVal = prediction[0][4]

        fileName = img2.split(".")[0]
        yPos = fileName.split("-")[0]
        xPos = fileName.split("-")[1]
        pointsFound.append([CATAGORIES[currentMax], currentMax, xPos, yPos])
        print(prediction)
        print("Found at XPos: " + xPos + "Found at YPos:" + yPos)

    print(pointsFound)

    printfoundsegments(pointsFound)


def printfoundsegments(pointsFound):

    data = []

    data1 = {'imageName' : 'test', 'imageWidth' : 2000, 'imageHeight' : 2000, 'tileWidth' : 50, 'tileHeight' : 50}

    for symbol in pointsFound:
        data.append({'objectFound': symbol[0], 'enumNumber': symbol[1], 'xPos': symbol[2], 'yPos': symbol[3]})

    with open('SatelliteLoader/Assets/StreamingAssets/JSONFiles/ClassifiedLevel.json', 'w', encoding='utf-8') as f:
        json.dump({'metaData' : data1, 'tileArray': data}, f, ensure_ascii=False, indent=4)



print(identifySegment())

