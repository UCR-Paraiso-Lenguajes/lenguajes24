"use client";
import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
//Para manejar tokens
const { default: jwt_decode } = require("jwt-decode");
//import jwt_decode from "jwt-decode";
//https://stackoverflow.com/questions/75353079/cant-import-jwt-decode-in-javascript-file-in-react-project

export default function  VerifyComponent({children,}: {children: React.ReactNode}) {
    const router = useRouter();       
    useEffect(()=>{        

        //Verificamos si existe un item en el localStorage
        const loginToken = sessionStorage.getItem("loginToken");        
        if(!loginToken){
            //nos vamos al login
            router.push("/../admin");
            return;       
        }

        //El token existe:
        try {
            const tokenFormat = jwt_decode(loginToken);

            //Tiempo del token (patear al login)
            const todayDate = Date.now() / 1000;
            //El tiempo de expiracion se indica desde el authcontroller
                //si el tiempo de expiracion es menor a la fecha actual
            if(tokenFormat.exp < todayDate){
                //nos vamos al login
                router.push("/../admin");
                return;            
            }
        }catch(error){
            //si se produce un error al decodificar el token:
            router.push("/../admin");
            return;
        }

    //cada vez que router cambie (cambiar de pagina) verificamos el token
    },[router])
    return children;
}