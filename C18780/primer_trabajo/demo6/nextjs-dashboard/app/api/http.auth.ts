export interface LoginModel {
    UserName: string;
    Password: string;
}

export interface AuthenticatedResponse {
    token: string;
}

let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7099';

export async function login(user: LoginModel): Promise<AuthenticatedResponse> {
    const response = await fetch(`${environmentUrl}/api/Auth/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user),
    });

    if (!response.ok) {
        throw new Error('Failed to login');
    }

    const data: AuthenticatedResponse = await response.json();
    return data;
}
