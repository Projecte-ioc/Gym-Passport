from jwcrypto import jwk

from utils_tea_2 import Connexion

con = Connexion()
# contenido = {
# "ciphertext": "_tQAp5q1FGgis9TEEULA232JE-XWSlU-HW1aNPb1pe0EmednFi6ZzuoaRfKoO8nhFVygC4IuaeI5WBSUyuFns1Y9TyGy2qDxvQyzbx1WaX9qiEv1AukDNJfTCPgZ2ZqyaebqOs-7EvX12zfau0IU5KgdcEOYGMa9Y_OrXH_cLznpHxLSvPtGmnr6tc89APt8qKRA1irfvqbYNYz6Z2pFLfTcLRPYF1JWMuG6XOmDJPUVhvdxbFgYOkCyvsTAGa_XJlQ1KjAa3F0856WEYWW_yqCcL3DBpAQaNVePxA7xnQTsQnrNQR-ilDQKrCbxrUpkYF3nHT4z54hycjrUGHtSIY_pri1R0gK0VUXUUdCmHdmqds7_UlPEOBfDKnTz6hSoOUSZwGNihF38r-pi2HWE7wha2QjdenhLBzURt-TeVKC6k7XrPiFnlVETR6GfwYC5",
# "encrypted_key": "v68u4RfcUfcozOOZQhBPcRkX5EPghPbZStfCTbRGLv2d8NqnpuNQc5jh9CjWow5jdUp05gUB0pFARJ1gvA68nbKYc7YZmKBR",
# "iv": "8cwlInMspD_Mekj0rj3nSA",
# "protected": "eyJhbGciOiJBMjU2S1ciLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIn0",
# "tag": "iPozK36-dR8iyagtqxvTflfCo3XIafDZxaTK7nMcZmQ"}
SK = con.convert_password_base64()
# result = con.descipher_content(content=contenido, SK=SK)

cif = con.custom_serialize(token="Hola", SK=SK)
print(cif)
