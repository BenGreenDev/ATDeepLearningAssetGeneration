import sys

import tensorflow
import tensorflow.keras
import pandas

print(f"Tensor Flow Version: {tensorflow.__version__}")
print(f"Keras Version: {tensorflow.keras.__version__}")
print()
print(f"Python Version: {sys.version}")
print(f"Pandas Version: {pandas.__version__}")
print("GPU Availability: ", "Available" if tensorflow.test.is_gpu_available() else "Not Available")

from tensorflow.python.client import device_lib
print(device_lib.list_local_devices())

print(tensorflow.test.is_gpu_available())