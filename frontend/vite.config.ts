import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import fs from 'fs'
import path from 'path'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    https: {
      key: fs.readFileSync(path.resolve(__dirname, '../SistemaEmpresas/certificado/localhost-key.pem')),
      cert: fs.readFileSync(path.resolve(__dirname, '../SistemaEmpresas/certificado/localhost.pem')),
    },
    proxy: {
      '/api': {
        target: 'https://localhost:5001',
        changeOrigin: true,
        secure: false, // Para desenvolvimento com certificados self-signed
      }
    }
  }
})
