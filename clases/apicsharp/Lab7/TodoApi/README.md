
dotnet add package MySqlConnector --version 2.3.6


docker pull mysql
docker run --name some-mysql -e MYSQL_ROOT_PASSWORD=123456 -d mysql
3306
root
123456

dotnet 8.0.100
docker pull mysql
docker run --name some-mysql -e MYSQL_ROOT_PASSWORD=123456 -d mysql
node 10.2.4
cristianguillenmendez@Cristians-MacBook-Pro TodoApi % docker run --name some-mysql -e MYSQL_ROOT_PASSWORD=123456 -d mysql
1119ec27aefec75a5c2f7294a36d0f2c3765a07c2e2a2c88de61447b40a87eee
cristianguillenmendez@Cristians-MacBook-Pro TodoApi % dotnet --version
8.0.100
cristianguillenmendez@Cristians-MacBook-Pro TodoApi % npm -version
10.2.4
cristianguillenmendez@Cristians-MacBook-Pro TodoApi % 


podman build -t api .
podman run --name api -d  -e ASPNETCORE_ENVIRONMENT=Development -e DB="Server=192.168.100.81;Database=mysql;Uid=root;Pwd=123456;" -p 8080:8080  api