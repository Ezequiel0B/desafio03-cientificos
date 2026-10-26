const cientificoEjemplo = {
    nombre: "Albert",
    apellido: "Einstein",
    nacionalidad: "Alemana",
    fechaNacimiento: "1879-03-14",
    email: "albert.einstein@cientificos.test",
    especialidadIds: [1, 4]
};

const especialidadEjemplo = {
    nombre: "Astronomía",
    descripcion: "Estudio de los cuerpos celestes"
};

const descubrimientoEjemplo = {
    titulo: "Efecto fotoeléctrico",
    descripcion: "Explicación cuántica de la emisión de electrones",
    fechaDescubrimiento: "1905-03-17",
    lugar: "Berna, Suiza",
    cientificoId: 1
};

const investigacionEjemplo = {
    titulo: "Teoría de la relatividad",
    areaEstudio: "Física teórica",
    fechaInicio: "1905-01-01",
    fechaFin: null,
    estado: "En curso",
    cientificoId: 1
};

const endpoints = [
    {
        grupo: "Científicos",
        metodo: "GET",
        ruta: "/api/cientificos",
        titulo: "Listar científicos"
    },
    {
        grupo: "Científicos",
        metodo: "GET",
        ruta: "/api/cientificos/{id}",
        titulo: "Detalle de un científico"
    },
    {
        grupo: "Científicos",
        metodo: "POST",
        ruta: "/api/cientificos",
        titulo: "Crear científico",
        cuerpo: cientificoEjemplo
    },
    {
        grupo: "Científicos",
        metodo: "PUT",
        ruta: "/api/cientificos/{id}",
        titulo: "Actualizar científico",
        cuerpo: cientificoEjemplo
    },
    {
        grupo: "Científicos",
        metodo: "DELETE",
        ruta: "/api/cientificos/{id}",
        titulo: "Eliminar científico"
    },
    {
        grupo: "Científicos",
        metodo: "POST",
        ruta: "/api/cientificos/{id}/especialidades/{especialidadId}",
        titulo: "Asignar especialidad"
    },
    {
        grupo: "Científicos",
        metodo: "DELETE",
        ruta: "/api/cientificos/{id}/especialidades/{especialidadId}",
        titulo: "Quitar especialidad"
    },

    {
        grupo: "Especialidades",
        metodo: "GET",
        ruta: "/api/especialidades",
        titulo: "Listar especialidades"
    },
    {
        grupo: "Especialidades",
        metodo: "POST",
        ruta: "/api/especialidades",
        titulo: "Crear especialidad",
        cuerpo: especialidadEjemplo
    },

    {
        grupo: "Descubrimientos",
        metodo: "GET",
        ruta: "/api/descubrimientos",
        titulo: "Listar descubrimientos"
    },
    {
        grupo: "Descubrimientos",
        metodo: "GET",
        ruta: "/api/cientificos/{cientificoId}/descubrimientos",
        titulo: "Descubrimientos por científico"
    },
    {
        grupo: "Descubrimientos",
        metodo: "POST",
        ruta: "/api/descubrimientos",
        titulo: "Crear descubrimiento",
        cuerpo: descubrimientoEjemplo
    },
    {
        grupo: "Descubrimientos",
        metodo: "PUT",
        ruta: "/api/descubrimientos/{id}",
        titulo: "Actualizar descubrimiento",
        cuerpo: descubrimientoEjemplo
    },
    {
        grupo: "Descubrimientos",
        metodo: "DELETE",
        ruta: "/api/descubrimientos/{id}",
        titulo: "Eliminar descubrimiento"
    },

    {
        grupo: "Investigaciones",
        metodo: "GET",
        ruta: "/api/investigaciones",
        titulo: "Listar investigaciones"
    },
    {
        grupo: "Investigaciones",
        metodo: "GET",
        ruta: "/api/investigaciones/estado/{estado}",
        titulo: "Filtrar por estado"
    },
    {
        grupo: "Investigaciones",
        metodo: "POST",
        ruta: "/api/investigaciones",
        titulo: "Crear investigación",
        cuerpo: investigacionEjemplo
    },
    {
        grupo: "Investigaciones",
        metodo: "PUT",
        ruta: "/api/investigaciones/{id}",
        titulo: "Actualizar investigación",
        cuerpo: investigacionEjemplo
    },
    {
        grupo: "Investigaciones",
        metodo: "DELETE",
        ruta: "/api/investigaciones/{id}",
        titulo: "Eliminar investigación"
    }
];

const elementos = {
    baseUrl: document.querySelector("#baseUrl"),
    menu: document.querySelector("#menuEndpoints"),
    metodo: document.querySelector("#metodo"),
    titulo: document.querySelector("#tituloEndpoint"),
    ruta: document.querySelector("#rutaEndpoint"),
    parametros: document.querySelector("#parametros"),
    seccionCuerpo: document.querySelector("#seccionCuerpo"),
    cuerpo: document.querySelector("#cuerpo"),
    estado: document.querySelector("#estadoHttp"),
    duracion: document.querySelector("#duracion"),
    urlEjecutada: document.querySelector("#urlEjecutada"),
    resultado: document.querySelector("#resultado"),
    conexion: document.querySelector("#estadoConexion")
};

let endpointActual = endpoints[0];

function obtenerBaseUrl() {
    return elementos.baseUrl.value.trim().replace(/\/+$/, "");
}

function construirMenu() {
    let grupoActual = "";

    endpoints.forEach((endpoint, indice) => {
        if (endpoint.grupo !== grupoActual) {
            grupoActual = endpoint.grupo;

            const grupo = document.createElement("div");
            grupo.className = "grupo";

            const tituloGrupo = document.createElement("h3");
            tituloGrupo.textContent = grupoActual;

            grupo.appendChild(tituloGrupo);
            elementos.menu.appendChild(grupo);
        }

        const boton = document.createElement("button");
        boton.type = "button";
        boton.className = "endpoint";
        boton.dataset.indice = indice;

        const insignia = document.createElement("span");
        insignia.className = `insignia ${endpoint.metodo}`;
        insignia.textContent = endpoint.metodo;

        const titulo = document.createElement("span");
        titulo.textContent = endpoint.titulo;

        boton.append(insignia, titulo);

        boton.addEventListener("click", () => seleccionarEndpoint(indice));

        elementos.menu.lastElementChild.appendChild(boton);
    });
}

function seleccionarEndpoint(indice) {
    endpointActual = endpoints[indice];

    document.querySelectorAll(".endpoint").forEach(boton => {
        boton.classList.toggle(
            "activo",
            Number(boton.dataset.indice) === indice
        );
    });

    elementos.metodo.textContent = endpointActual.metodo;
    elementos.metodo.className = `metodo ${endpointActual.metodo}`;
    elementos.titulo.textContent = endpointActual.titulo;
    elementos.ruta.textContent = endpointActual.ruta;

    crearParametros(endpointActual.ruta);

    const tieneCuerpo = endpointActual.cuerpo !== undefined;
    elementos.seccionCuerpo.hidden = !tieneCuerpo;

    elementos.cuerpo.value = tieneCuerpo
        ? JSON.stringify(endpointActual.cuerpo, null, 2)
        : "";
}

function crearParametros(ruta) {
    elementos.parametros.replaceChildren();

    const coincidencias = [...ruta.matchAll(/\{([^}]+)\}/g)];

    coincidencias.forEach(coincidencia => {
        const nombre = coincidencia[1];

        const label = document.createElement("label");
        label.textContent = nombre;

        const input = document.createElement("input");
        input.className = "parametro";
        input.dataset.parametro = nombre;
        input.value = nombre === "estado" ? "En curso" : "1";

        elementos.parametros.append(label, input);
    });
}

async function enviarPeticion() {
    let rutaFinal = endpointActual.ruta;

    document.querySelectorAll("[data-parametro]").forEach(input => {
        rutaFinal = rutaFinal.replace(
            `{${input.dataset.parametro}}`,
            encodeURIComponent(input.value.trim())
        );
    });

    const url = obtenerBaseUrl() + rutaFinal;

    const opciones = {
        method: endpointActual.metodo,
        headers: {
            Accept: "application/json"
        }
    };

    if (endpointActual.cuerpo !== undefined) {
        try {
            JSON.parse(elementos.cuerpo.value);
        } catch {
            alert("El cuerpo no contiene un JSON válido.");
            return;
        }

        opciones.headers["Content-Type"] = "application/json";
        opciones.body = elementos.cuerpo.value;
    }

    elementos.urlEjecutada.textContent =
        `${endpointActual.metodo} ${url}`;

    const inicio = performance.now();

    try {
        const respuesta = await fetch(url, opciones);
        const milisegundos = Math.round(performance.now() - inicio);
        const texto = await respuesta.text();

        mostrarRespuesta(respuesta, texto, milisegundos);
    } catch (error) {
        elementos.estado.textContent = "Sin conexión";
        elementos.estado.style.background = "#c62828";
        elementos.duracion.textContent = "";

        elementos.resultado.textContent =
            "No se pudo conectar con la API.\n\n" +
            "Comprueba que la API esté ejecutándose en:\n" +
            obtenerBaseUrl() +
            "\n\nDetalle: " +
            error.message;
    }
}

function mostrarRespuesta(respuesta, texto, milisegundos) {
    elementos.estado.textContent =
        `${respuesta.status} ${respuesta.statusText}`;

    if (respuesta.status < 300) {
        elementos.estado.style.background = "#218838";
    } else if (respuesta.status < 500) {
        elementos.estado.style.background = "#e67817";
    } else {
        elementos.estado.style.background = "#c62828";
    }

    elementos.duracion.textContent = `${milisegundos} ms`;

    if (!texto) {
        elementos.resultado.textContent =
            "(Respuesta sin contenido)";
        return;
    }

    try {
        const datos = JSON.parse(texto);
        elementos.resultado.textContent =
            JSON.stringify(datos, null, 2);
    } catch {
        elementos.resultado.textContent = texto;
    }
}

async function probarConexion() {
    elementos.conexion.textContent = "Comprobando...";

    try {
        const respuesta = await fetch(
            obtenerBaseUrl() + "/api/especialidades"
        );

        elementos.conexion.textContent = respuesta.ok
            ? "API conectada"
            : `Respuesta ${respuesta.status}`;
    } catch {
        elementos.conexion.textContent = "Sin conexión";
    }
}

document
    .querySelector("#btnEnviar")
    .addEventListener("click", enviarPeticion);

document
    .querySelector("#btnConexion")
    .addEventListener("click", probarConexion);

construirMenu();
seleccionarEndpoint(0);