# Xadrez de Console em C# - Projeto de Estudo OOP

Este projeto é um jogo de xadrez totalmente funcional desenvolvido para rodar no terminal, criado como parte dos meus estudos em C# e Programação Orientada a Objetos. O foco principal foi aplicar os pilares da OOP para criar um código limpo, organizado e extensível.

## 📸 Preview da Interface


a b c d e f g h

+-----------------+
8 |r|n|b|q|k|b|n|r| 8
7 |p|p|p|p|p|p|p|p| 7
6 | | | | | | | | | 6
5 | | | | | | | | | 5
4 | | | | | | | | | 4
3 | | | | | | | | | 3
2 |P|P|P|P|P|P|P|P| 2
1 |R|N|B|Q|K|B|N|R| 1
+-----------------+
a b c d e f g h

Turno: Brancas | Status: Em andamento Última Jogada: N/A Capturadas (pelas Brancas): Capturadas (pelas Pretas):
📊 Perf: Threads: 10 | CPU: 0.2s | Mem: 35MB
Selecione a peça de origem (ex: e2):


## ✨ Funcionalidades

* **Jogo de Xadrez Completo:** Implementação de todas as regras básicas do xadrez.
* **Validação de Movimentos:** Cada peça conhece suas próprias regras de movimento.
* **Lógica de Xeque e Xeque-Mate:** O jogo identifica e anuncia estados de xeque e o fim da partida por xeque-mate.
* **Captura de Peças:** Sistema de contagem e exibição de peças capturadas.
* **Interface de Console:** Layout limpo e funcional para jogar diretamente no terminal.
* **Monitor de Performance:** Um painel que exibe informações sobre o uso de threads, CPU e memória da aplicação em tempo real.

## 🧠 Conceitos de OOP Aplicados

Este projeto foi uma oportunidade para praticar os seguintes conceitos:

* **Abstração:** A classe `ChessPiece` foi criada como uma classe base abstrata, definindo um contrato comum (`Symbol`, `GetValidMoves`) que todas as peças concretas devem seguir.
* **Herança:** Cada peça específica (`Pawn`, `Rook`, `King`, etc.) herda da classe `ChessPiece`, reutilizando a lógica comum e implementando seu comportamento único.
* **Polimorfismo:** O motor do jogo interage com uma lista de objetos `ChessPiece` sem precisar saber o tipo exato de cada uma. Ao chamar o método `GetValidMoves()`, a versão correta do método é executada para cada tipo de peça, demonstrando o polimorfismo em ação.
* **Encapsulamento:** As classes como `Board` e `GameState` encapsulam seus dados e lógicas internas, expondo apenas os métodos necessários para interagir com elas de forma segura (ex: `MovePiece`, `IsInCheck`).

## 🚀 Como Executar

**Pré-requisitos:**
* [.NET 8.0 (ou superior)] (https://dotnet.microsoft.com/download/dotnet/8.0)

**Passos:**
1.  Clone este repositório:
    ```bash
    git clone <URL_DO_SEU_REPOSITORIO>
    ```
2.  Navegue até a pasta do projeto:
    ```bash
    cd <NOME_DA_PASTA_DO_PROJETO>
    ```
3.  Execute a aplicação:
    ```bash
    dotnet run
    ```

## 📁 Estrutura do Projeto

O código está organizado da seguinte forma para seguir os princípios de separação de responsabilidades:

* **/Engine:** Contém a lógica principal e o fluxo do jogo (`GameEngine`).
* **/Graphics:** Responsável por toda a renderização no console (`ConsoleRenderer`).
* **/Models:** Contém as classes que representam os dados do jogo, como o tabuleiro (`Board`), o estado (`GameState`) e as peças (`Pieces`).

---

*Projeto desenvolvido por [Luiz Carlos] como parte do aprendizado em C#.*
