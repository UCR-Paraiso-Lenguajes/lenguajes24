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
      router.push("/../admin"); // Redirect immediately if no token
      return;
    }

    try {
      const tokenFormat = jwtDecode(loginToken);
      const todayDate = Date.now() / 1000;

      if (tokenFormat.exp && tokenFormat.exp < todayDate) {
        setIsValidToken(false);
        router.push("/../admin"); // Redirect immediately if token is expired
      } else {
        setIsValidToken(true);
      }
    } catch (error) {
      setIsValidToken(false);
      router.push("/../admin"); // Redirect immediately if token is invalid
    } finally {
      setIsVerifying(false);
    }
  };

  useEffect(() => {
    verifyToken();

    const handleStorageChange = () => {
      verifyToken();
    };

    window.addEventListener("storage", handleStorageChange);

    return () => {
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