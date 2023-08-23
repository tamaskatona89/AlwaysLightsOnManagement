# AlwaysLightsOnManagement
AlwaysLightsOn Company's Working and Issue Management Software

<h3 align="right"> Cégnév: MINDIG FÉNYES KFT</h3>
<b><i> A Mindig Fényes kft köztéri lámpák szervizelését végzi.
Webszerveren keresztül várják a meghibásodások bejelentését, de ezt használják a kollégák nap elején az elvégzendő munkák lekérdezéséhez 
illetve a titkárnő a havi statisztikákat is itt készíti majd el.</i></b>

### Web modul
A közvilágítással kapcsolatos problémát regisztráció nélkül bárki jelezhet egy egyszerű webes űrlap kitöltésével.<br/>
Ehhez elegendő egy címet megadnia. Elküldése után a számítógépen található adatbázisban rögzítésre kerül a beérkezett cím, <br/>
és a beérkezés időpontja (ez utóbbit a rendszer automatikusan generálja).<br/>
A beküldő visszajelzést kap a sikeres bejelentésről.
### Console 1 modul
Munkakezdéskor a szervízes kollégák egy egyszerű szöveges felületen keresztül tudják lekérdezni a még el nem végzett munkák listáját.<br/>
A munkalista vagy irányítószámok alapján, vagy budapesti cím esetén kerület szerint válogatva is elérhető.<br/>
(Pl. ha a munkás a 3333-as irányítószámot adja meg, akkor Terpes településről érkezett hibák listáját kapja, ha viszont 106-ot ad <br/>
meg lekérdezésként, akkor az összes VI. kerületi hiba megjelenik: 1061, 1062, 1063... )<br/>
Ha a lekérdezéshez 100 alatti értéket írunk, akkor pedig azoknak a feladatoknak listáját kapjuk, amik az adott értéknél régebben kerültek be az adatbázisba. <br/>
(Pl. a 66 beírása esetén a mai dátumhoz képest 67, 68, 69... nappal korábban érkezett hibák listája jelenik meg.)
### Console 2 modul
A nap végén a munka lezárásakor ki kell választani az elvégzet javítás típusát (izzócsere, lámpabúra, vezeték javítás...) <br/>
A javítás dátuma automatikusan rögzül. Ekkor kell tárolni a munkát végző kolléga azonosítóját is.
### DESKTOP-UI modul
Az adatok feldolgozásáért felelős munkatárs az alábbi lekérdezéseket tudja elvégezni grafikus felületen:<br/>
 - Egy munkatárs által elvégzett munkák listája
 - Egy adott hónapban elvégzett összes munka
 - Feladatok típusa szerint csoportosított lista <br/>
A megjelenített adatokat ki lehet exportálni egy külön fájlba is. (XML)<br/>
Ezeket az adatokat megfelelő UI objektumok felhasználásával jelenítsük meg (pl. listbox, radiobutton, button...)
### Teszt modul
A megrendelőnk szeretné folyamatosan ellenőrizni, hogy az általunk fejlesztett alkalmazás modulok megfelelően működnek e az általunk elvégzett frissítések után is. <br/>
Ennek érdekében kérte, hogy az alkalmazás tartalmazzon unit-teszteket is. <br/> 
Ehhez nem szükséges külön alkalmazást készíteni, megelégszik a fejlesztőkörnyezet segítségével futtatható tesztekkel.
<br/>
<br/>
<br/>
## Működés
<b><u>WEBFELÜLETEN:</u></b>   Csak CÍM megadása kell !  + Visszajelzés: Sikeres bejelentés! (DBTárolás: CÍM + Bejelentés ideje) <br/>
<b><u>Console1:</u></b>       El nem végzett munkák listája (ISElvégezve =0) + IR.szám szűrés (4 jegy) <br/>
                + 3 jegynél külön kezelni a BP kerületek szűrést <br/>
                + 2 jegyű szűrés Korábbi napokra országosan <br/>
<b><u>Console2:</u></b>       munka kiválaszt + hozzáadom a munkavégzés tipust ( izzócsere, lámpabúra, vezeték javítás... <br/>
                A javítás dátuma automatikusan rögzül. +a munkát végző kolléga azonosítóját is. ) <br/>
<b><u>Desktop:</u></b>        Dolgozó DropDown, alatta DolgozóMunkái, Dátumszűrő + MunkaTipus-szűrő >> Adott hónap BEFEJEZETT összes munka-listája + Export To XML Gomb... <br/>
<br/>
<br/>
<br/>

<h3 align="right">                                                    =============== <br/>
                                                    |  DataMODEL  |<br/>
                                                    ===============</h3>

ReportedIssues Tábla (a bejelentő ebbe ír bele)
==================================================================================================================
<Issue_ID>, ZIP_CODE,Address, Reported_DateTime (TimeStamp), IS_Fixed (boolean)

Worker Tábla (cég által előre adott)
==================================================================================================================
<Worker_ID>, FullName

WorkTypes Tábla (cég által előre adott: izzócsere, lámpabúra, vezeték javítás...)
==================================================================================================================
<WorkType_ID>, WorkType_Description

ActualWork Tábla (a nap végén a munkás ebbe rögzíti az elvégzett melót)
==================================================================================================================
<ActualWork_ID>, ++ReportedIssues.Issue_ID++, ++WorkTypes.WorkType_ID++, ++Worker.Worker_ID++, Fixing_DateTime (TimeStamp)

