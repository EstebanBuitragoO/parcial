Ó
RC:\Users\Esteban\Downloads\Parcial\Parcial\src\Application\UseCases\CreateOrder.cs
	namespace 	
Application
 
. 
UseCases 
; 
public 
static 
class 
CreateOrderUseCase &
{		 
public

 

static

 
Order

 
Execute

 
(

  
string

  &
customer

' /
,

/ 0
string

1 7
product

8 ?
,

? @
int

A D
qty

E H
,

H I
decimal

J Q
price

R W
)

W X
{ 
var 
order 
= 
OrderService  
.  !
CreateOrder! ,
(, -
customer- 5
,5 6
product7 >
,> ?
qty@ C
,C D
priceE J
)J K
;K L
return 
order 
; 
} 
} 