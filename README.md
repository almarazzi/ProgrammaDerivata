# ProgrammaDerivata

**ProgrammaDerivata** è un progetto in **C#** che utilizza **ANTLR4** per analizzare espressioni matematiche e calcolarne la derivata in modo simbolico. Questo progetto è pensato per chi vuole avere un sistema modulare e riutilizzabile per il calcolo simbolico delle derivate, completo di suite di test per garantire correttezza e stabilità del codice. Il progetto è rilasciato sotto licenza **MIT**.

---

## Descrizione del progetto

Il progetto permette di calcolare la derivata di una funzione matematica inserita come stringa, ad esempio `x^2 + sin(x)`.  
Il processo avviene in più fasi: il testo della funzione viene prima analizzato tramite ANTLR4, che costruisce un **albero sintattico** (AST), poi un visitor personalizzato attraversa l’albero e applica le regole di derivazione simbolica per restituire una nuova espressione, che rappresenta la derivata della funzione di partenza.

Esempio concreto:
Input: f(x) = x^2 + 3x + sin(x)
Output: f'(x) = 2x + 3 + cos(x)

---

Il progetto è organizzato in modo modulare, separando la logica di parsing e derivazione dai test e dagli esempi, per rendere più semplice l’estensione futura e l’integrazione in altri progetti.

---

## Funzionalità principali

- 📘 **Parser matematico** basato su ANTLR4 per leggere espressioni matematiche complesse.  
- ✏️ **Calcolo simbolico** delle derivate per polinomi, funzioni trigonometriche e funzioni elementari.  
- 🧩 **Struttura modulare**, che permette di aggiungere facilmente nuovi tipi di funzioni o regole di derivazione.  
- ✅ **Suite di test automatizzati** per verificare la correttezza dei calcoli.  

---

## Architettura del progetto

Il progetto è strutturato in diverse cartelle e file principali:

- **`ProgrammaDerivate/`** → contiene la logica principale del calcolo delle derivate e la grammatica ANTLR4 (`.g4`).  
- **`TestDerivate/`** → contiene i test unitari per verificare il corretto funzionamento del parser e del calcolo delle derivate.  
- **`ProgrammaDerivate.sln`** → solution per aprire il progetto in Visual Studio o altri IDE compatibili con .NET.  
- **`LICENSE.txt`** → file della licenza MIT.  

---

## ANTLR4

ANTLR4 (**ANother Tool for Language Recognition**) è un potente generatore di parser che consente di definire una grammatica per un linguaggio (in questo caso, le espressioni matematiche), e generare automaticamente un lexer e un parser in C#.  

Il flusso con ANTLR4 funziona così:  

1. Si scrive una **grammatica `.g4`** che definisce i numeri, gli operatori (`+ - * / ^`) e le funzioni matematiche (`sin`, `cos`, `exp`, ecc.).  
2. ANTLR4 genera un **lexer**, che trasforma la stringa in token, e un **parser**, che costruisce un **AST** (Abstract Syntax Tree).  
3. Un **visitor personalizzato** attraversa l’AST e applica le regole di derivazione simbolica, ad esempio:  
   - `(d/dx) x^n = n*x^(n-1)`  
   - `(d/dx) sin(x) = cos(x)`  
   - `(d/dx) cos(x) = -sin(x)`  
4. Infine, il visitor restituisce una nuova stringa che rappresenta la derivata della funzione iniziale.  

---
## Diagramma del flusso
(Digramma del flusso) 

---
## Tecnologie utilizzate

- **C# (.NET)** → linguaggio di programmazione principale.  
- **ANTLR4** → generatore di parser per analisi delle espressioni.  
- **xUnit / NUnit / MSTest** → framework per i test automatici.

---

## Installazione

### Prerequisiti

- [.NET SDK](https://dotnet.microsoft.com/) versione 6 o superiore.  
- [ANTLR4 runtime per C#](https://www.antlr.org/).  
- IDE consigliato: Visual Studio o Visual Studio Code con estensione ANTLR4.

