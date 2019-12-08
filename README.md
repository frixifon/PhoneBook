<h1>Книга контактов</h1>
<h3>Документация по первой части задания (PHP + XML)</h3>

<h5>Задание</h5>
Создать веб-страницу книги контактов, с помощью которой, пользователь сможет добавлять контакты, а так же сможет искать нужный контакт по определённому параметру.

<h5>Этапы</h5>
• Создать XML файл, содержащий 2-3 логичесских диапозона.<br>
• XML файл должен содержать Id, имя, фамилию, номер телефона и адрес электронной почты.<br>
• Отображение данных в таблице HTML, при помощи PHP.<br>
• Придумать 3 функции.<br>
    * Поиск по различным параметрам. <br>
	* Добавление нового контакта. <br>
	* Редактирование уже существующего контакта. <br>
	
<h5>XML файл</h5>
```
<?xml version="1.0" encoding="utf-8"?>
<raamat>
    <kontakt id="1">
		<nimi>Vladimir</nimi>
		<perekonnanimi>Trohhalev</perekonnanimi>
		<telefon>53225533</telefon>
		<email>frixifon@gmail.com</email>
    </kontakt>
	<kontakt id="2">
		<nimi>Vadim</nimi>
		<perekonnanimi>Vassiltseko</perekonnanimi>
		<telefon>53004455</telefon>
		<email>qwilf@gmail.com</email>
    </kontakt>
    <kontakt id="3">
		<nimi>Oleg</nimi>
		<perekonnanimi>Avtomobilev</perekonnanimi>
		<telefon>213124256</telefon>
		<email>bibika12@gmail.com</email>
    </kontakt>
    <kontakt id="4">
		<nimi>Nikita</nimi>
		<perekonnanimi>Vinogradov</perekonnanimi>
		<telefon>59989987</telefon>
		<email>vin@gmail.com</email>
    </kontakt>
	<kontakt id="5">
		<nimi>Danil</nimi>
		<perekonnanimi>Gritsenko</perekonnanimi>
		<telefon>55555588</telefon>
		<email>123@mail.ru</email>
	</kontakt>
	<kontakt id="5">
		<nimi>Kevin</nimi>
		<perekonnanimi>Konstantinov</perekonnanimi>
		<telefon>52145211</telefon>
		<email>konstantinov@gmail.com</email>
	</kontakt>
</raamat>
```
