# Banco Atl�ntico
## Desenvolvido por Kelvim Guidini


[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

Projeto de gerenciamento de caixas eletr�nicos.

## O projeto cont�m

- Documenta��o dos EndPoints no swagger
- Real-time com SignalR
- Teste da unidade (para o servi�os dp caixa eletr�nico)
- Banco de dados usaod � inMemory (dados s�o gerado ao iniciar o API)
- HealthCheck para API
- FrameWork UI - Bootstrap
- Autentica��o JWT - Bearer

Ao rodar o projeto � esperado que o usu�rio consiga, por uma interface ver a lista dos caixas eletr�nicos ativos e a quantidade de notas dispon�veis em cada um deles. Exibindo alerta caso o estoque de alguma nota fique abaixo de 5. Existe bot�o para desativar o caixa eletr�nico. 
Foi criado EndPoint de saque. Ao realizar saque, deve ser atualizado a quantidade de notas no painel de gerenciamento em tempo real. O caixa eletr�nico deve ser capaz de determinar a quantidade de n�mero de cadas uma das notas
necess�rio para totalizar o valor, de modo a minimizar a quantidade de c�lulas entregues, tentando manter o minimo de 5 notas.


## Tequinologias

Ferramentas utilizadas no projeto:

- [AngularJS]
- [Bootstrap]
- [ASP.NET Core]

## Como rodar o projeto

Para iniciar a aplica��o executar no visual studio.

Entrar no ender�o https://localhost:44353/swagger/index.html para ver os detalhes dos endpoints
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

Entrar no ender�o http://localhost:4200/ 

