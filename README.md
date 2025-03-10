Titulo del juego: En este caso, estoy utilizando una librería de pago para generar una animación en un TMT_Text, se llama Febucci, por si la quieren investigar, el funcionamiento es el siguiente
- Se agrega un TMT_Text.
- Se le agrega el componente Text Animator, en la parte de Edit Effects & Actions encuentras todos los efectos disponibles.
- Por defecto viene el efecto de wave junto con un waitfor para esperar un tiempo para iniciar el efecto, si quieren conservar el efecto solo es cambiar la palabra Title game y poner el título que se desee, si lo quieren cambiar pueden mirar un poco como funciona la librería, pero con el ejemplo debería bastar para entenderla un poco.
Manejo de resoluciones: Las resoluciones se obtienen directamente de unity. En el script ManagementData en la función SetStartingData se crean por primera vez las resoluciones disponibles.
Manejo de idioma: En la raíz de todo el proyecto, se encuentra una carpeta ExternalAssets, ahí se encuentra el archivo original de los idiomas en formato xlsx, una vez finalizado el archivo debe exportarse en formato CSV UTF-8 delimitado por comas, el funcionamiento es el siguiente:
- Son 2 columnas principales: ID y Descripción
    - ID: Es el ID representativo del texto
    - Descripción: Es una breve descripción del texto, para saber a dónde hace referencia.
- Las columnas después de descripción se utilizan para agregar el idioma que se requiera, como por ejemplo la columna English y Español.
- Para utilizar esto hay que tener 2 cosas en cuenta:
    - Se debe agregar el idioma en ManagementData en el enum TypeLanguage.
    - Se debe agregar el componente ManagementLanguage al TMP_Text donde se vaya a utilizar, solo se debe asignar el ID correspondiente a la columna ID para que funcione.
Manejo de escenas: Para agregar una escena nueva, se debe hacer lo siguiente.
- Agregar el nombre en GameManager en el enum TypeScene.
- Se debe llamar a la función ChangeSceneSelector con la escena a la que se desea cambiar, esto realizará automáticamente los efectos de fundido a pantalla "negra" y el fadeOut del audio para que no haya un corte brusco.
- También se puede llamar atrevés de la función GameManagerHelper, la cual se utiliza para llamar a los métodos del GameManager atrevés del UI, por ejemplo si quieren cambiar de escena con un Button en pantalla, agregarían el script GameManagerHelper y llamarían a la función ChangeScene, y le pasarían el index correspondiente a la escena, este se encuentra en el TypeScene del GameManager.
Sonidos: Esto funciona de forma similar al manejo de escenas, se debe agregar un GameManagerHelper y llamar a la función PlayASoundButton, para que tenga un pitch random entre 0.9f y 1.1f; También pueden hacer uso de la función PlayASound con las diferentes sobrecargas para lo que se necesite, aunque si es un Button es recomendable pasarle la función PlayASoundButton.
Menú de pausa: Para invocarlo se hace directamente desde el GameManagerHelper de la misma forma que se cambia de escena, solamente que en este caso, le pasas el index 1, perteneciente al menú de opciones.
