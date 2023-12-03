from jwcrypto import jwk

from utils_tea_2 import Connexion
con = Connexion()
contenido = {
    "ciphertext": "BmuppMFRxsucZ1fTz5F4wDbo1LMNFEbUtngKDZZ1RPrl0qlfNDueXL-bSp3ZFKSMk3gx4hwSlzQ5855buVPYzMvWsHehj3F8vYTzcie2OHiBhgNqMoalOt392AA_sXCVAJcRnOWRqOp725qk6pRpNXcECVVV77uhP1BWVr53TR3b9ClhEc1ZwV5OtFJvh-2_XfLQ1QnpDpzYCIg9E07P1V6KswpNHU2vrZlYGr4Lu-ei0aMTG6cpVuxRJPb0I6iqkzJvVnD2Qb36qSmQxDRviAsCVJY_YvliTn7BlobUq9Z2HSmA0L8hzg2v9p-qBW-JoIjH37bvqidma1qEtjOcQBIUK8AzCmzP6icIE6wdNQmKbwyp6Cxe5xEeHTp87kKl6ZFTS1-0VHyhNWWmWRAVp8r1fCQfcD5X1Gqgoj9Y7l5O1iHeNFNcQ4Xknhf15P6j",
    "encrypted_key": "PfXf9M_Wy_kDhTgVzN5nGEW8np2zzxhmRwt95oAzHnSFravScoUiM23gs4MyhzDQtIoTOO5Gt29zBHtuoyjhuMNBzXycKamO",
    "iv": "O2BTnn3TtZk_EO2dH4kJ2g", "protected": "eyJhbGciOiJBMjU2S1ciLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIn0",
    "tag": "KxEdrrgxsAOceJDNuJFBvYnGWx311pU704NeH1B76tA"}
SK = con.convert_password_base64()
print(type(SK))
result = con.descipher_content(content=contenido, SK=SK)
print(result)
