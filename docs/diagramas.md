graph TD
    subgraph Frontend [Cliente / Frontend]
        UI[Vistas / Interfaz de Usuario]
        API_Client[Cliente HTTP / Servicios API]
        UI --> API_Client
    end
    subgraph Backend [Servidor / Backend - API REST]
        direction TB
        Ctrl[Controladores / Controllers]
        Serv[Lógica de Negocio / Services]
        Repo[Acceso a Datos / Repositories] 
        Ctrl --> Serv
        Serv --> Repo
    end
    subgraph Database [Persistencia]
        DB[(Base de Datos)]
    end
    API_Client -- HTTP Requests / JSON --> Ctrl
    Repo --> DB
Este diagrama representa cómo interactúan las capas del sistema, desde la interfaz de usuario hasta la base de datos.




    
    
