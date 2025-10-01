
# QMOVE 

Este projeto utiliza Azure Container Registry (ACR) e Azure Container Instance (ACI) para armazenar e executar a aplicação, além de um container MySQL em nuvem para o banco de dados.

## Integrantes

- Hellen Marinho Cordeiro RM 558841
- Heloisa Alves de Mesquita RM 559145

## Como executar 

#### Clone o projeto
```bash
git clone https://github.com/hellomesq/ContainerQmove
cd ContainerQmove
code .
```
#### Execução no Gitbash
```bash
cd /c/Users/SeuUsuario/NomeDaPasta
az login
```
#### Acessar o Banco de Dados
O banco MySQL está rodando em um container no Azure. Para acessar e verificar o conteúdo das tabelas, use o seguinte comando no terminal:
```bash
docker run -it --rm mysql:8 mysql -h aci-qmove-db.eastus.azurecontainer.io -P 3306 -u root -p
senha: root123
```
#### Criação do Grupo de Recursos
```bash
./build.sh
```
#### Deploy da aplicação
```bash
./deploy.sh
```
#### Acesse a aplicação no navegador
```bash
http://aci-qmove-api.eastus.azurecontainer.io:8080/swagger/index.html 
```


