import os
from utils_tea_2 import Connexion
import jwt
from flask import Flask
db = Connexion()
app = Flask(__name__)
app.config['SECRET_KEY'] = os.getenv("SK")
message = jwt.encode({
            'user_name': 'meri33usr',
            'pswd_app': 'Meri33pswd'
        }, app.config['SECRET_KEY'], algorithm='HS256')

mssg = db.cipher_content(message)
print(mssg)
