export interface LoginModel {
    UserName: string;
    Password: string;
}

export interface AuthenticatedResponse {
    token: string;
}

export async function login(user: LoginModel): Promise<AuthenticatedResponse> {
    const response = await fetch("https://localhost:7099/api/Auth/login", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user),
    });

    if (!response.ok) {
        console.log("Fallo login");
        throw new Error('Failed to login');
    }

    const data: AuthenticatedResponse = await response.json();
    return data;
}
