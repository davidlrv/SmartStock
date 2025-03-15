document.addEventListener('DOMContentLoaded', function () {

    const passwordInput = document.getElementById('clave');
    const passwordHelp = document.getElementById('passwordHelp');

    passwordInput.addEventListener('input', function () {
        const password = passwordInput.value;
        document.getElementById('clave2').value = "";
        document.getElementById('passwordHelp2').textContent = '';

        // Expresión regular para validar una contraseña segura
        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&-=.;/])[A-Za-z\d@$!%*?&-=.;/]{8,}$/;

        if (passwordRegex.test(password)) {
            passwordHelp.textContent = "La contraseña es segura.";
            passwordHelp.classList.add("text-muted");
            passwordHelp.classList.remove("text-danger");
            if (document.getElementById('passwordHelp2').textContent == 'Las contraseñas coinciden') {
                document.getElementById('btnAcceder1').disabled = false;
                //document.getElementById('btnAcceder1').classList.add("btn-secondary");
                //document.getElementById('btnAcceder1').classList.remove("btn-secondaryGris");
                
            }
        } else {
            passwordHelp.textContent = "La contraseña debe tener al menos 8 caracteres, una letra mayúscula, una letra minúscula, un número y un carácter especial.";
            passwordHelp.classList.add("text-danger");
            passwordHelp.classList.remove("text-muted");
            document.getElementById('btnAcceder1').disabled = true;
            //document.getElementById('btnAcceder1').classList.add("btn-secondaryGris");
            //document.getElementById('btnAcceder1').classList.remove("btn-secondary");
        }
    });




    // validar la clave 2
    var clave2 = document.getElementById('clave2');

    clave2.addEventListener('input', function () {
        // Obtener el campo de error para mostrar mensajes
        var error = document.getElementById('passwordHelp2');
        var clave = document.getElementById('clave').value;
        // Validar que las contraseñas coincidan
        if (clave !== clave2.value) {
            error.textContent = "Las contraseñas no coinciden.";
            error.classList.add("text-danger"); // Agregar la clase text-danger para marcar el error en rojo 
            error.classList.remove("text-muted");
            document.getElementById('btnAcceder1').disabled = true;
           // document.getElementById('btnAcceder1').classList.add("btn-secondaryGris");
            //document.getElementById('btnAcceder1').classList.remove("btn-secondary");
        } else {
            error.textContent = "Las contraseñas coinciden"; // Limpiar el mensaje de error si coinciden
            error.classList.remove("text-danger"); // Quitar la clase text-danger si las contraseñas coinciden
            error.classList.add("text-muted");
            if (document.getElementById('passwordHelp').textContent == 'La contraseña es segura.') {
                document.getElementById('btnAcceder1').disabled = false;
                //document.getElementById('btnAcceder1').classList.add("btn-secondary");
               //document.getElementById('btnAcceder1').classList.remove("btn-secondaryGris");
             }
        }
    });

    

});