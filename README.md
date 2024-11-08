# ptm_dev_test

1. Utilizando o Swagger para testar os Endpoints:
Cadastre um usuário no endpoint "api/Users", depois de cadastrar o usuário, prossiga para o endpoint de login/autenticação. 
Lá, basta inserir o usuário recém-criado e obter o token que será gerado. Depois disso, insira o token nas solicitações dos Endpoints de exames.

Observação: Os únicos endpoints que requerem token são os de exames, enquanto os de usuário e autenticação não necessitam.

2. Usando o Postman para Testar a API
Exemplo Completo de Requisição no Postman
Para fazer uma requisição para exames siga os passos abaixo:

```plaintext
URL: https://localhost:7261/api/Exames
Headers:
    accept: */*
    Authorization: Bearer <seu_token_jwt>
    Content-Type: application/json
Body (Selecione raw e JSON como o tipo de conteúdo):
{
  "nome": "Luiz",
  "idade": 20,
  "genero": "Masculino"
}
```
