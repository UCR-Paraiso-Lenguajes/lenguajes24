'use client'
import { useRouter } from 'next/navigation';
import { useEffect, useState } from 'react';
import { LoginModel, login } from '../api/http.auth';
import '../ui/styles/login.css';
import 'bootstrap/dist/css/bootstrap.min.css';

export default function Page() {
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const router = useRouter();

    const handleSubmit = async (event: React.FormEvent | React.MouseEvent) => {
        event.preventDefault();
        setIsLoading(true);

        const user: LoginModel = { UserName: userName, Password: password };

        try {
            const response = await login(user);

            document.cookie = `token=${response.token}; max-age=${60 * 5}; path=/`

            // Redirecciona al dashboard o a la página principal
            router.push('/admin');
        } catch (error) {
            console.error('Error logging in:', error);
            alert('Invalid username or password');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <>
            <div className="container-fluid">
                <div className="row no-gutter">
                    <div className="col-md-6 d-none d-md-flex bg-image"></div>

                    <div className="col-md-6 bg-light">
                        <div className="login d-flex align-items-center py-5">

                            <div className="container">
                                <div className="row">
                                    <div className="col-lg-10 col-xl-7 mx-auto">
                                        <h3 className="display-4">Abacaxi</h3>
                                        <p className="text-muted mb-4">The best online store!.</p>
                                        <form onSubmit={handleSubmit}>
                                            <div className="form-group mb-3">
                                                <input id="inputEmail" type="email" placeholder="Email address" required autoFocus className="form-control rounded-pill border-0 shadow-sm px-4" onChange={(e) => setUserName(e.target.value)} />
                                            </div>
                                            <div className="form-group mb-3">
                                                <input id="inputPassword" type="password" placeholder="Password" required className="form-control rounded-pill border-0 shadow-sm px-4 text-primary" onChange={(e) => setPassword(e.target.value)} />
                                            </div>
                                            <div className="custom-control custom-checkbox mb-3">
                                                <input id="customCheck1" type="checkbox" defaultChecked className="custom-control-input" />
                                                <label htmlFor="customCheck1" className="custom-control-label">Remember password</label>
                                            </div>

                                            {/* Inputs y demás elementos del formulario */}
                                            <button type="submit" className="btn btn-primary btn-block text-uppercase mb-2 rounded-pill shadow-sm">
                                                {isLoading ? 'Loading...' : 'Sign in'}
                                            </button>

                                            <div className="text-center d-flex justify-content-between mt-4">
                                                <p>Forgot your password? <a href="" className="font-italic text-muted"><u>register here!</u></a></p>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}