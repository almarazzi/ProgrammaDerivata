grammar Expr;

prog: expr EOF ;      // Punto di ingresso

expr
    : expr '+' expr        #SommaEspressione
    | expr '-' expr        #SottrazioneEspressione
    | expr '*' expr        #MoltiplicazioneEspressione
    | expr '/' expr        #DivisioneEspressione
    | expr '^' expr        #PotenzaEspressione
	| expr 'sqrt' expr     #RadiceEspressione
    | 'y=' '(' expr ')'    #DerivataEspressione
    | func '(' expr ')'    #FunzioneEspressione
    | '(' expr ')'         #ParentesiEspressione
	| '-' NUMBER			#NumeroNeg
    | NUMBER               #NumeroEspressione
    | ID                   #CostanteEspressione
    ;

// Funzioni matematiche
func
    : 'sin' | 'cos' | 'tan' | 'log' NUMBER | 'ln' | 'e^'
    ;

// Token
ID      : 'x' ;                              // Identificatore
NUMBER  : [0-9]+ ('.' [0-9]+)? ;            // Numeri interi o decimali
WS      : [ \t\r\n]+ -> skip ;              // Spazi bianchi ignorati