import requests
import matplotlib.pyplot as plt
import matplotlib.image as mpimg
import cv2
import os
import shutil
import math
import numpy as np
import webcolors as wc
from random import uniform

def getImage(lat, long, width, height, zoom):
    app_key = "AX4i8VcSScEGfRDyqDtRJJfY01lKUKfb"
    latitude = lat
    longitude = long
    width = width
    # Plus 60 to remove watermark
    watermark_height = 60
    height = height + watermark_height
    image_type = "sat"
    image_format = "png"
    zoom_level = zoom

    request_url = "https://www.mapquestapi.com/staticmap/v4/getmap?key=" + app_key + "&size=" + str(width) + "," + str(height) + "&type=" + image_type + "&imagetype=" + image_format + "&zoom=" + str(zoom_level) + "&scalebar=false&traffic=false&center=" + str(latitude) + "," + str(longitude)

    response = requests.get(request_url, stream=True)

    with open('img.png', 'wb') as out_file:
        shutil.copyfileobj(response.raw, out_file)
    del response

    img = cv2.imread("img.png")
    crop_img = img[0:height - watermark_height, 0:width]
    cv2.imwrite("img.png", crop_img)

    print("Image gathered and saved")

    return

def sliceImage(height, width, stride_height, stride_width, filepath, fileLoc):

    img = cv2.imread(filepath)
    img_height, img_width = img.shape[:2]

    num_y_increments = (img_height / height) * (height / stride_height)
    num_x_increments = (img_width / width) * (width / stride_width)

    num_y_increments = math.ceil(num_y_increments)
    num_x_increments = math.ceil(num_x_increments)

    print(num_y_increments)
    print(num_x_increments)

    i = 0

    for y in range(0, num_y_increments):
        for x in range(0, num_x_increments):
            print("current y : " + str(y * stride_height) + " current x : " + str(x * stride_width))
            crop_img = img[(y * stride_height):(y * stride_height) + height,
                       (x * stride_width):(x * stride_width) + width]
            cv2.imwrite("Test/" + '' + str(y * stride_height) + "-" + str(x * stride_width) + ".jpg", crop_img)
            i += 1
    return

def amountOfColour(filepath):
    img = cv2.imread(filepath, -1)
    avg_color_per_row = np.average(img, axis=0)
    avg_color = np.average(avg_color_per_row, axis=0)

    requested_colour = (int(avg_color[0]), int(avg_color[1]), int(avg_color[2]))
    actual_name, closest_name = get_colour_name(requested_colour)

    print(closest_name)
    return

def closest_colour(requested_colour):
    min_colours = {}
    for key, name in wc.css3_hex_to_names.items():
        r_c, g_c, b_c = wc.hex_to_rgb(key)
        rd = (r_c - requested_colour[0]) ** 2
        gd = (g_c - requested_colour[1]) ** 2
        bd = (b_c - requested_colour[2]) ** 2
        min_colours[(rd + gd + bd)] = name
    return min_colours[min(min_colours.keys())]

def get_colour_name(requested_colour):
    try:
        closest_name = actual_name = wc.rgb_to_name(requested_colour)
    except ValueError:
        closest_name = closest_colour(requested_colour)
        actual_name = None
    return actual_name, closest_name


x, y = uniform(-180, 180), uniform(-90, 90)

print(x,y)

#To 5 decimal places
house_lat = 51.49119
house_long = -0.48178


getImage(house_lat, house_long, 2000, 2000, 17)


sliceImage(50,50,50,50,"img.png", "//NewFolder")
