#!/usr/bin/env python
# coding: utf-8

# ## Notebook 1
# 
# 
# 

# In[75]:


import base64
from azure.storage.blob import BlobServiceClient
import requests
from pyspark.sql import SparkSession


# In[64]:


blob_service_client = BlobServiceClient.from_connection_string("DefaultEndpointsProtocol=https;AccountName=;AccountKey=;EndpointSuffix=core.windows.net")
blob_client = blob_service_client.get_blob_client(container="ppsEditor", blob="Bliss.png")
encodedBlob = base64.b64encode(blob_client.download_blob().readall()).decode('utf-8')


# In[65]:


resp = requests.post(
    url = "https://ppsEditor.azurewebsites.net/api/Function1?",
    json = {
        "image": encodedBlob,
        "width": "320",
        "height": "320"
    }
)


# In[76]:


rescaledb64 = resp.text
# rescaledpng = base64.b64decode(rescaledb64).decode('utf-16')
rescaledpng = base64.b64decode(rescaledb64)
# base64.b64encode(rescaledb64).decode('utf-8')

with open("/tmp/output_image.png", "wb") as temp_file:
    temp_file.write(rescaledpng)

spark = SparkSession.builder.getOrCreate()
spark.sparkContext.addFile("/tmp/output_image.png")

mssparkutils.fs.mv("file:/tmp/output_image.png", "abfss://ppsEditor@adlsmnitbi.dfs.core.windows.net/BlissRescaled.png")

# mssparkutils.fs.put("abfss://ppsEditor@adlsmnitbi.dfs.core.windows.net/BlissRescaled.png", rescaledpng, True)


# In[77]:


resp = requests.post(
    url = "https://ppsEditor.azurewebsites.net/api/Function2?",
    json = {
        "image": encodedBlob
    }
)


# In[78]:


rescaledb64 = resp.text
# rescaledpng = base64.b64decode(rescaledb64).decode('utf-16')
rescaledpng = base64.b64decode(rescaledb64)
# base64.b64encode(rescaledb64).decode('utf-8')

with open("/tmp/output_image.jpg", "wb") as temp_file:
    temp_file.write(rescaledpng)

spark = SparkSession.builder.getOrCreate()
spark.sparkContext.addFile("/tmp/output_image.jpg")

mssparkutils.fs.mv("file:/tmp/output_image.jpg", "abfss://ppsEditor@adlsmnitbi.dfs.core.windows.net/BlissRecoded.jpg")

# mssparkutils.fs.put("abfss://ppsEditor@adlsmnitbi.dfs.core.windows.net/BlissRescaled.png", rescaledpng, True)


# In[84]:


blob_service_client = BlobServiceClient.from_connection_string("DefaultEndpointsProtocol=https;AccountName=;AccountKey=;EndpointSuffix=core.windows.net")
blob_client = blob_service_client.get_blob_client(container="ppsEditor", blob="Metadata.jpg")
encodedBlob = base64.b64encode(blob_client.download_blob().readall()).decode('utf-8')


# In[85]:


resp = requests.post(
    url = "https://ppsEditor.azurewebsites.net/api/Function3?",
    json = {
        "image": encodedBlob
    }
)


# In[86]:


print(resp.text)

