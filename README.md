# Tech Challenge 3 - Grupo 15

Projeto realizado pelo **Grupo 15** da turma da FIAP de Arquitetura de Sistemas .NET com Azure


## Autores

||
|--|
| Evandro Prates Silva |
| Caio Vinícius Moura Santos Maia |
| Guilherme Castro Batista Pereira |
| Luis Gustavo Gonçalves Reimberg |


## DeleteContact

### Tecnologias Utilizadas
- .NET 8
- Dapper
- RabbitMQ
- FluentValidation
- XUnit
- MediatR
- Moq

Dentro da arquitetura de microsserviços desenvolvida para este tech challenge, este projeto realiza a função de deletar os contatos, seguindo o passo a passo abaixo:

### API
- Receber a requisição
- Validar os dados da requisição e a existência do contato a ser deletado
- Atualizar o status do contato/registro no banco de dados
- Enviar a requisição para a respectiva fila de exclusão

### Worker
- Consumir a fila de exclusões
- Realizar a validação adicional para garantir a integridade do contato/registro no banco de dados
- Exclui o contato
