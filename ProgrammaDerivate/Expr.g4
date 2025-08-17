grammar Expr;

expr:  numeri ('*' | '/') operazione  	#DivPer 
    ;

operazione:     ID ('^' numeri)?  			#Potenza
    ;

numeri: numeri op=('*' |'/') numeri				#DivPerinterni
	|	numeri op=('+' |'-') numeri				#SommaMenointerni
	|   '-' NUMBER								#negativiNumeri
	|	NUMBER                             		#Number
    |   '(' numeri ')'                        	#Parent
    ;

ID      : 'x';                               // Identificatore 'x'
NUMBER  : [0-9]+ ('.' [0-9]+)?;             // Numeri interi o decimali
WS      : [ \t\r\n]+ -> skip;               // Spazi bianchi (ignorati)


