import '@/app/ui/global.css';
import { WebSocketProvider } from '@/app/hooks/WebSocketContext';

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body>
        <WebSocketProvider>{children}</WebSocketProvider>
      </body>
    </html>
  );
}
