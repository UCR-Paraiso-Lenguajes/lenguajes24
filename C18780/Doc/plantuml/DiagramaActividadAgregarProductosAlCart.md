@startuml
!define ICON_URL https://raw.githubusercontent.com/tupadr3/plantuml-icon-font-sprites/v2.2.0
!includeurl ICON_URL/common.puml

|Cliente|
start
:Inicio de Checkout;
if (¿Productos en Carrito?) then (Sí)
  :Mostrar Detalles de Productos;
  :Mostrar Resumen del Pedido;
  if (¿Dirección de Facturación Seleccionada?) then (Sí)
    :Mostrar Método de Pago;
    :Seleccionar Método de Pago;
    if (¿Método de Pago Seleccionado?) then (Sí)
      :Procesar Pago;
      if (¿Pago en Efectivo?) then (Sí)
        :Esperar Confirmación Administrador;
        :Mostrar Modal de Confirmación;
      else (No)
        :Ingresar Número de Confirmación Sinpe;
        :Confirmar Pago Sinpe;
      endif
      :Finalizar Compra;
    else (No)
      stop
    endif
  else (No)
    :Ingresar Dirección de Facturación;
  endif
else (No)
  :Redirigir a la Página de Productos;
endif
stop
@enduml
