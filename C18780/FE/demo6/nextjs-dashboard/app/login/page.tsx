'use client'
import { useRouter } from 'next/navigation';
import { useState } from 'react';
import { LoginModel, login } from '../api/http.auth';
import { jwtDecode } from 'jwt-decode';
import '../styles/Login.css';
export default function Page() {
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [showModal, setShowModal] = useState(false);

    const router = useRouter();

    const handleSubmit = async (event: React.FormEvent | React.MouseEvent) => {
        event.preventDefault();
        setIsLoading(true);

        const user: LoginModel = { UserName: userName, Password: password };

        try {
            const response = await login(user);
            const decodedToken = jwtDecode(response.token);
            const expirationTime = decodedToken.exp ? decodedToken.exp : 0;
            const time = Math.floor(Date.now() / 1000);
            const cookieMaxTime = expirationTime - time;
            document.cookie = `token=${response.token}; max-age=${cookieMaxTime}; path=/`

            router.push('/admin');
        } catch (error) {
            setShowModal(true);
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
                                        {showModal &&
                                            <><div className="alert alert-danger d-flex align-items-center" role="alert">
                                                <svg className="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="Danger:">
                                                    <use xlinkHref="#exclamation-triangle-fill" />
                                                </svg>
                                                <div>
                                                    Incorrect mail or password
                                                </div>
                                            </div></>
                                        }
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