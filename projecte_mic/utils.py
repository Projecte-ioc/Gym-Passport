import psycopg2


class Connexion:
    USER = 'isard'
    PASSWORD = 'pirineus'
    HOST = '127.0.0.1'
    PORT = 5432
    DATABASE = 'gympassportdb'
    SK = 'PROBANDOprobando'

    def get_connection_values(self):
        db_params = {
            'dbname': self.DATABASE,
            'user': self.USER,
            'password': self.PASSWORD,
            'host': self.HOST,
            'port': self.PORT
        }
        return db_params

    def get_connection_to_db(self):
        connex = psycopg2.connect(**self.get_connection_values())
        cursor = connex.cursor()
        return connex, cursor
