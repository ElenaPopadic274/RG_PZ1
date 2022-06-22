# RAČUNARSKA GRAFIKA(SOFTVER SA KRITIČNIM ODZIVOM U ELEKTROENERGETSKIM SISTEMIMA) | COMPUTER GRAPHICS (CRITICAL RESPONSE SOFTWARE IN POWER SYSTEMS)
# School project - Faculty of Technical Science

Cilj prvog predmetnog zadatka je iscrtavanje grafa elektroenergetske mreže. Graf mreže je potrebno iscrtati na osnovu 
Geographic.xml fajla i sam graf aproksimira mrežu na ortogonalni prikaz. Prvo je potrebno da se površina za iscrtavanje 
podeli na (“zamišljene”) podeoke, a što je više takvih podeoka definisano, to će prikaz biti detaljniji. 
Potom se učitavaju koordinate iz xml fajla i crtaju se entiteti mreže, tako što se aproksimiraju na najbliži podeok 
na površini za crtanje. 

Entiteti mreže se iscrtavaju tako što se iscrtava slika (grafički element) koja će predstavljati datu vrstu entiteta 
(Substation, Node, Switch). Za svaki grafički element se prikazuje ToolTip sa informacijom koji entitet se tu nalazi: </br>
1-a: Entiteti mreže se aproksimiraju na najbliži podeok i u tom slučaju se mogu preklapati. Ako dođe do preklapanja, 
na datom mestu se iscrtava neka posebna sličica koja označava grupu, a u ToolTip-u se prikazuju informacije o svakom 
entitetu koji se tu nalazi. (2 poena)</br>
1-b: Entiteti mreže se aproksimiraju na najbliži slobodni podeok, bez preklapanja. U ovom slučaju treba voditi računa 
o minimalnom broju podeoka kako bi bilo prostora za sve. Predlog: minimum 100x100 (3 poena)</br>

Vodovi, koji spajaju entitete, se crtaju kao prave linije i ukoliko je potrebno, linija mora da skreće samo pod pravim 
uglom. Posmatraju se samo Start i End Nodes u linijama, a Vertices se ignorišu. Iscrtavaju se samo one linije čiji Start 
i End Node postoje u kolekcijama entiteta. Ostali vodovi se ignorišu. Treba ignorisati ponovno iscrtavanje vodova izmedju 
dva ista entiteta. Linija uvek mora da kreće iz centra entiteta, ne iz gornjeg levog ugla (pozicije iscrtavanja) entiteta: </br>
2-a: Vod se iscrtava kao najkraća putanja između dva entiteta (bilo koja najkraća). Ukoliko je na zadatom mestu već
iscrtan vod, ne crtati novi preko njega. Ako dođe do preseka vodova, označiti presek. (4 poena) </br>
2-b: Nalazi se najkraći mogući put BEZ presecanja sa već postojećim iscrtanim vodovima (kroz upotrebu BFS algoritma). 
U drugom prolazu se iscrtavaju vodovi za koje u prvom prolazu nije bilo moguće naći put bez presecanja i tada se i oni 
iscrtavaju uz označavanje tačaka preseka. Predlog: Algoritam započeti od neka dva entiteta koja imaju najmanju udaljenost 
na gridu. Naći ih automatski ili ručno. (6 poena) </br>

Desnim klikom na vod između dva entiteta treba ponuditi opciju da entiteti povezani tim vodom budu obojeni različitim
bojama od ostalih kako bi korisnik znao koji su entiteti povezani tim vodom. Potrebno je omogućiti zumiranje prikaza 
mreže tako da se pomoću skrol-barova može pomerati pogled nad zumiranom delu mreže, kao i da se prikaz mreže može „odzumirati“. 

Na vrhu prozora, potrebno je ponuditi korisniku opcije da se iscrtani graf mreže može dopuniti oblicima i/ili tekstom: </br>
a. Draw Ellipse: izborom ove opcije, potom klikom na desni taster miša na površini Canvas-a otvara se novi prozor u okviru 
kojeg se unose i biraju atributi elipse (dužine poluprečnika, debljina konturne linije, boje) posle čega se iscrtava 
elipsa po zadatim atributima. Takođe, opciono ponuditi korisniku da unese tekst koji će biti napisan na površini iscrtane 
elipse i izbor boje teksta (veličina teksta je fiksirana). </br>
b. Draw Polygon: izborom ove opcije, potom klikom na desni taster miša na Canvas određuju se tačke koje će ograničiti 
površinu poligona. Kada se sve tačke odrede, levim klikom na površini Canvas-a otvara se novi prozor u okviru kojeg se 
unose i biraju atributi poligona (debljina konturne linije, boje) posle čega se iscrtava poligon po zadatim atributima. 
Takođe, opciono ponuditi korisniku da unese tekst koji će biti napisan na površini iscrtanog poligona i izbor boje teksta 
(veličina teksta je fiksirana). </br>
c. Add Text: izborom ove opcije, potom klikom na desni taster miša na površini Canvas-a otvara se novi prozor u okviru 
kojeg se unose i biraju atributi teksta: sam tekst, njegova boja i veličina. </br>
d. Undo: poništava iscrtavanje poslednjeg oblika ili teksta (nakon Clear vraća sve što je obrisano) </br>
e. Redo: vraća prethodno uklonjen oblik ili tekst </br>
f. Clear: prazni Canvas od svih iscrtanih oblika ili teksta </br> 

Napomene: </br>
-Tooltip-ovi prikazuju ID i ime entiteta, a prikazuju se i za veze (vodove). </br>
-Svi oblici (i tekst) se iscrtavaju tako da im je gornji levi ugao pozicija gde je pokazivačem miša kliknuto da bi se 
inicirala akcija crtanja. Kada se kaže “na površinu Canvas-a” tu se misli i na prethodno nacrtane oblike i tekst (mogu se 
iscrtavati jedni preko drugih). </br>
-Svaki od iscrtanih oblika i tekst treba da ima opciju da se klikom levim tasterom miša na njega mogu menjati njegovi 
atributi izgleda (boje i debljine konturna linije, a za tekst njegova boja i veličina). </br>

 --------------------------------------------------------------------------------------------------------------------------------------------------------------------

The goal of the first subject task is to draw a graph of the electric power network. The network graph needs to be plotted 
based on the Geographic.xml file and the graph itself approximates the network to an orthogonal view. First, it is 
necessary to divide the drawing area into ("imaginary") divisions, and the more such divisions are defined, the more 
detailed the presentation will be. The coordinates from the xml file are then loaded and the network entities are drawn 
by approximating them to the nearest division on the drawing surface. 

Network entities are drawn by drawing an image (graphic element) that will represent a given type of entity (Substation, 
Node, Switch). For each graphic element, a ToolTip is displayed with information on which entity is located there:</br>
1-a: Network entities are approximated to the nearest division and in that case they can overlap. If an overlap occurs, 
a special thumbnail identifying the group is drawn in the given place, and the ToolTip displays information about each 
entity located there. (2 points) </br>
1-b: Network entities are approximated to the nearest free division, without overlap. In this case, the minimum number 
of divisions should be taken into account so that there is room for everyone. Suggestion: minimum 100x100 (3 points) </br>


Lines connecting the entities are drawn as straight lines and if necessary, the line must turn only at right angles. 
Only Start and End Nodes in lines are observed, and Vertices are ignored. Only those lines whose Start and End Node exist 
in entity collections are drawn. Other lines are ignored. The redrawing of lines between two same entities should be 
ignored. The line must always start from the center of the entity, not from the upper left corner (drawing position) of 
the entity: </br>
2-a: The line is drawn as the shortest path between two entities (any shortest). If a line has already been drawn in the
given place, do not draw a new one over it. If the lines intersect, mark the section. (4 points)</br>
2-b: There is the shortest possible path WITHOUT intersection with already existing drawn lines (through the use of BFS 
algorithm). In the second passage, lines are drawn for which it was not possible to find a path without intersections in 
the first passage, and then they are also drawn with marking of intersection points. Suggestion: The algorithm starts 
from some two entities that have the smallest distance on the grid. Find them automatically or manually. (6 points) </br>


By right-clicking on the line between the two entities, you should be offered the option to have the entities connected 
by that line colored differently from the others so that the user knows which entities are connected by that line. It is 
necessary to enable the zoom of the grid view so that the scroll bars can be used to move the view over the zoomed part of
the grid, as well as the grid view can be "zoomed out".

At the top of the window, it is necessary to offer the user the option that the drawn network graph can be supplemented 
with forms and / or text: </br>
a. Draw Ellipse: by selecting this option, then right-clicking on the Canvas surface, a new window opens in which the
attributes of the ellipse are entered and selected (radius length, contour line thickness, color) and then the ellipse 
is drawn according to the given attributes. Also, optionally offer the user to enter the text that will be written on 
the surface of the drawn ellipse and choose the color of the text (the size of the text is fixed). </br>
b. Draw Polygon: by selecting this option, then right-clicking on Canvas, the points that will limit the area of 
the polygon are determined. When all the points are determined, a left click on the surface of Canvas opens a new window 
in which the attributes of the polygon (thickness of the contour line, color) are entered and selected, after which the 
polygon is drawn according to the given attributes. Also, optionally offer the user to enter the text that will be written 
on the surface of the drawn polygon and choose the color of the text (the size of the text is fixed). </br>
c. Add Text: by selecting this option, then right-clicking on the Canvas surface, a new window opens in which text 
attributes are entered and selected: the text itself, its color and size. </br>
d. Undo: cancels the deletion of the last shape or text (after Clear returns everything that was deleted) </br>
e. Redo: returns the previously removed shape or text </br>
f. Clear: empty Canvas of all drawn shapes or text </br>


Notes:
-Tooltips display entity ID and name, and are also displayed for connections (lines).
-All shapes (and text) are drawn so that their upper left corner is the position where the mouse pointer is clicked to initiate the drawing action. When we say "on the surface of Canvas", we also mean previously drawn shapes and text (they can be drawn on top of each other).
-Each of the drawn shapes and text should have the option that by clicking the left mouse button on it, its appearance attributes can be changed (color and thickness of the contour line, and for the text its color and size).
