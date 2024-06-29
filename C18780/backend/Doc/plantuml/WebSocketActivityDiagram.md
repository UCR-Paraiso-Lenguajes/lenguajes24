@startuml
| Usuario |
start
:Usuario entra a la tienda;
| Admin |
if (¿Es admin?) then (sí)
  :Admin se autentica con user y password;
  :Backend utiliza JWT;
  :JWT enviado al frontend y guardado en cookie;
  :Admin crea o borra anuncios;
else (no)
  | Cliente |
  :Cliente accede a la tienda;
endif

if (¿Es cliente?) then (sí)
  :Cliente ve los 3 anuncios más recientes;
  if (¿Se crea un nuevo anuncio?) then (sí)
    :Cliente ve 4 anuncios (si estaba en la tienda);
  else (no)
    :Cliente ve 3 anuncios;
  endif
endif

| Usuario |
stop
@enduml
