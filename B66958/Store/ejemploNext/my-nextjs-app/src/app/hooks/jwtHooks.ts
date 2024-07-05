import { jwtDecode, JwtPayload } from 'jwt-decode';

interface LoginPayload extends JwtPayload {
  exp: number
}


export const decodeToken = (token: string): LoginPayload | null => {
  try {
    const decoded = jwtDecode<LoginPayload>(token);
    return decoded;
  } catch (error) {
    throw new Error("Error decrypting token");
  }
};

export const checkTokenDate = (seconds: number) => {
  const expiryDate = new Date(seconds * 1000);
  return expiryDate.getTime() > Date.now();
}