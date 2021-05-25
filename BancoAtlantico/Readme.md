# Banco Atlântico
## Desenvolvido por Kelvim Guidini


[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

Projeto de gerenciamento de caixas eletrônicos.

## O projeto contém

- Documentação dos EndPoints no swagger
- Real-time com SignalR
- Teste da unidade (para o serviços dp caixa eletrônico)
- Banco de dados usaod é inMemory (dados são gerado ao iniciar o API)
- HealthCheck para API
- FrameWork UI - Bootstrap
- Autenticação JWT - Bearer

Ao rodar o projeto é esperado que o usuário consiga, por uma interface ver a lista dos caixas eletrônicos ativos e a quantidade de notas disponíveis em cada um deles. Exibindo alerta caso o estoque de alguma nota fique abaixo de 5. Existe botão para desativar o caixa eletrônico. 
Foi criado EndPoint de saque. Ao realizar saque, deve ser atualizado a quantidade de notas no painel de gerenciamento em tempo real. O caixa eletrônico deve ser capaz de determinar a quantidade de número de cadas uma das notas
necessário para totalizar o valor, de modo a minimizar a quantidade de células entregues, tentando manter o minimo de 5 notas.


## Tequinologias

Ferramentas utilizadas no projeto:

- [AngularJS]
- [Bootstrap]
- [ASP.NET Core]

## Como rodar o projeto

Para iniciar a aplicação executar no visual studio.

Entrar no enderço https://localhost:44353/swagger/index.html para ver os detalhes dos endpoints
OBS: Para autenticar utilize o EndPoint /users/auth  passando:

```sh
{
  "username": "admin",
  "password": "admin"
}
```

Para executar o front, inicie o Angular. Pelo comando CLI:
```sh
ng 
serve
```

Entrar no enderço http://localhost:4200/ 

