"use client";
import { useEffect, useState, createContext, useContext } from 'react';
import { useRouter } from 'next/navigation';
import {jwtDecode} from 'jwt-decode';

const TokenContext = createContext({ isValidToken: false, isVerifying: true });

export const useTokenContext = () => useContext(TokenContext);

export default function VerifyToken({ children }: { children: React.ReactNode }) {
  const [isVerifying, setIsVerifying] = useState(true);
  const [isValidToken, setIsValidToken] = useState(false);
  const router = useRouter();

  const verifyToken = () => {
    const loginToken = sessionStorage.getItem("loginToken");

    if (!loginToken) {
      setIsValidToken(false);
      setIsVerifying(false);
      router.push("/../admin");
      return;
    }

    try {
      const tokenFormat: { exp: number } = jwtDecode(loginToken);
      const todayDate = Date.now() / 1000;

      if (tokenFormat.exp < todayDate) {
        setIsValidToken(false);
        sessionStorage.removeItem("loginToken");
        router.push("/../admin");
      } else {
        setIsValidToken(true);
      }
    } catch (error) {
      setIsValidToken(false);
      sessionStorage.removeItem("loginToken");
      router.push("/../admin");
    } finally {
      setIsVerifying(false);
    }
  };

  useEffect(() => {
    verifyToken();

    const intervalId = setInterval(() => {
      verifyToken();
    }, 60000); // Verificar cada 60 segundos

    const handleStorageChange = (event: StorageEvent) => {
      if (event.key === "loginToken" && event.newValue === null) {
        setIsValidToken(false);
        router.push("/../admin");
      }
    };

    window.addEventListener("storage", handleStorageChange);

    return () => {
      clearInterval(intervalId);
      window.removeEventListener("storage", handleStorageChange);
    };
  }, []);

  if (isVerifying) {
    return null;
  }

  return (
    <TokenContext.Provider value={{ isValidToken, isVerifying }}>
      {children}
    </TokenContext.Provider>
  );
}