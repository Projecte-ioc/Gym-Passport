import json
import os
from utils_tea_2 import Connexion
import jwt
from flask import Flask
db = Connexion()
app = Flask(__name__)

data = {
    "date": "20/11/2023",
    "done": False,
    "hour": 10,
    "name": "Zumba",
    "qty_max_attendes": 30,
    "whereisit": "Platja el Callao Mataró"
}
message = jwt.encode(data, "__PROBANDO__probando__", algorithm='HS256')

mssg = db.cipher_content(message)
mssg_dsc = db.decipher_content(mssg)
print(mssg)
print(mssg_dsc)
