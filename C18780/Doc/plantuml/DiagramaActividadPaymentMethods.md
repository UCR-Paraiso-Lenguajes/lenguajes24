@startuml
|Admin|
start
:Login;
:Go to PaymentMethodsPage;

|Frontend|
:Change payment method availability;

|Backend|
:Update payment method availability in backend;

|PaymentMethodsHub|
:Notify frontend (Checkout);

|Frontend|
:Hide payment method button in Checkout;
:Prevent user from using disabled payment method;

stop
@enduml
