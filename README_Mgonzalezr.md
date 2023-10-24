# gym-passport (server)
He creado el servidor con la máquina virtual ofrecida por la IOC en IsardVDI. He usado una
máquina ubuntu cliente 20.04.

El **procedimiento** seguido a sido el **siguiente**:
- he instalado [Apache](https://httpd.apache.org/download.cgi)
- lo he configurado para que apuntará a la IP publica de nuestro servidor.
- he instalado [NoIP](https://www.noip.com/es-MX) para el DNS Dinámico.
- he instalado [PostgreSQL](https://www.digitalocean.com/community/tutorials/how-to-install-and-use-postgresql-on-ubuntu-20-04-es).
- he creado el usuario isard, ya que es el usuario de la máquina.
- le he assignado el mismo password que el que tiene de acceso.
- me he descargado el fichero de configuración de VPN de la máquina virtual.
- me he descargado en mi máquina [WireGuard](https://www.wireguard.com/install/). Es necesario tener el WireWard instalado en la máquina local donde se vayan a ejecutar las aplicaciones ya que es lo que hace connexión con el servidor.
- le he importado el archivo de configuración de VPN
- Compruebo que puedo visualizar el archivo index.html de Apache accediendo des de mi máquina local
- reviso que este python y pip instalado e instalo las librerias que hay en el archivo requirements.txt
- importo la API la ejecuto y hago pruebas des de Postman.

### GET ALL
http://10.2.190.11:3000/clientes

### GET WITH FILTER
http://10.2.190.11:3000/cliente?nombre=Meri

### INSERT INTO
http://10.2.190.11:3000/addcliente?nombre=Paco&permisos=normal&usuario=pacopaco&pswd_app=pacopaco1

### UPDATE
http://10.2.190.11:3000/updatecliente?nombre=Pablo&permisos=admin&usuario=pablopablito7&pswd_app=pablopablito23

### DELETE WHERE
http://10.2.190.11:3000/deletecliente?id=15

MUCHAS GRACIAS.

MERITXELL GONZÁLEZ ROBERT
