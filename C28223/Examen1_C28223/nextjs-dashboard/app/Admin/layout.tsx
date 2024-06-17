import '@/app/ui/global.css';
import ValidationLoginSession from '../Security/SecureLogin';

export default function RootLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    return (
        <html lang="en">
            <body>
            <ValidationLoginSession>
                    {children}
            </ValidationLoginSession>
            </body>
        </html>
    );
}