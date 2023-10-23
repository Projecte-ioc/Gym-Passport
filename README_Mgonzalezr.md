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
![descargar_archivo_vpn.png](..%2Fdescargar_archivo_vpn.png)
- me he descargado en mi máquina [WireGuard](https://www.wireguard.com/install/)
- le he importado el archivo de configuración de VPN
- Compruebo que puedo visualizar el archivo index.html de Apache accediendo des de mi máquina local
- reviso que este python y pip instalado e instalo las librerias que hay en el archivo requirements.txt
- importo la API la ejecuto y hago pruebas des de Postman.

### GET ALL
![get_all.png](..%2Fget_all.png)

### GET WITH FILTER
![get_with_filter.png](..%2Fget_with_filter.png)

### INSERT INTO
![insert_into.png](..%2Finsert_into.png)

### UPDATE
![update.png](..%2Fupdate.png)

### DELETE WHERE
![delete_where.png](..%2Fdelete_where.png)

MUCHAS GRACIAS.

MERITXELL GONZÁLEZ ROBERT