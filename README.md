# CitasApp
Patrones GOF implementados
Factory () — selecciona el repositorio según el entorno (Development → JSON, Production → Memoria)RepositoryFactory
Decorator () — agrega logging con timestamp sin modificar el repositorio originalLoggingPacienteRepository
Observer (, ) — notifican automáticamente al confirmar una cita sin acoplar CitaService a los canales de notificaciónSmsObserverEmailObserver
