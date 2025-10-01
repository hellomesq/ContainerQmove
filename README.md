
# QMOVE 

Este projeto utiliza Azure Container Registry (ACR) e Azure Container Instance (ACI) para armazenar e executar a aplicação, além de um container MySQL em nuvem para o banco de dados.

## Integrantes

- Hellen Marinho Cordeiro RM 558841
- Heloisa Alves de Mesquita RM 559145

## Descrição e Benefícios para o Negócio

A gestão de frotas de motos em pátios de múltiplas filiais é um desafio operacional crítico enfrentado pela Mottu. Atualmente, o controle das motos é realizado majoritariamente de forma manual, gerando imprecisões, dificultando o acompanhamento em tempo real e impactando diretamente a eficiência das operações.

A solução implementada utiliza **QR Codes únicos em cada moto** e um **aplicativo móvel** para leitura e atualização dos dados da frota em tempo real. Isso permite:

- **Rastreabilidade e controle preciso da frota**, com informações de placa, modelo e localização da moto.  
- **Monitoramento em tempo real** do estado das motos, como “Disponível”, “Em Manutenção” ou “Pendências”.  
- **Alertas inteligentes** e log de alterações para maior segurança e rastreabilidade.  
- **Redução de erros humanos**, maior agilidade no atendimento e melhor alocação de recursos.  
- **Escalabilidade operacional**, permitindo o controle centralizado de mais de 100 filiais.

Em resumo, a aplicação transforma o processo manual de controle da frota em uma operação **digital, ágil e segura**, gerando ganhos significativos de eficiência e precisão para a Mottu.

---

## Como executar 

#### Clone o projeto
```bash
git clone https://github.com/hellomesq/ContainerQmove
cd ContainerQmove
code .
```
#### Acessar projeto no Gitbash
```bash
cd /c/Users/SeuUsuario/NomeDaPasta
az login
```
#### Banco de dados local para testes 
```bash
docker exec -it mysql-local mysql -u root -p
senha: root123
CREATE DATABASE qmove;
```
#### Criação do Grupo de Recursos no GitBash
```bash
./build.sh
```
#### Deploy da aplicação no GitBash
```bash
./deploy.sh
```
#### Acessar o Banco de Dados pelo CMD
O banco MySQL está rodando em um container no Azure. Para acessar e verificar o conteúdo das tabelas, use o seguinte comando no terminal:
```bash
docker run -it --rm mysql:8 mysql -h aci-qmove-db.eastus.azurecontainer.io -P 3306 -u root -p
senha: root123
```
#### Acesse a aplicação no navegador
```bash
http://aci-qmove-api.eastus.azurecontainer.io:8080/swagger/index.html 
```


