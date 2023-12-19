import json
import os
from utils_tea_2 import Connexion
import jwt
from flask import Flask
db = Connexion()
app = Flask(__name__)

data = {
    "address": "Carrer Llibertat, 43, Mataró (Barcelona) 08302",
    "phone_number": "658956423",
    "schedule": [
        "Lunes a Viernes 06h a 23h",
        "Sábado 09h a 16h",
        "Domingo 09h a 14h"
    ]
}
message = jwt.encode(data, "__PROBANDO__probando__", algorithm='HS256')

mssg = db.cipher_content(message)
print(mssg)
