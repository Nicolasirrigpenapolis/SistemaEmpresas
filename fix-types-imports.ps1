$files = Get-ChildItem -Path "c:\Projetos\SistemaEmpresas2\frontend\src" -Recurse -Include *.tsx,*.ts

foreach($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $original = $content
    
    # Corrigir imports de types
    $content = $content -replace "from '\.\.\/\.\.\/types\/produto'", "from '../../types'"
    $content = $content -replace "from '\.\.\/\.\.\/types\/dashboard'", "from '../../types'"
    $content = $content -replace "from '\.\.\/\.\.\/types\/emitente'", "from '../../types'"
    $content = $content -replace "from '\.\.\/\.\.\/types\/geral'", "from '../../types'"
    $content = $content -replace "from '\.\.\/\.\.\/types\/classificacaoFiscal'", "from '../../types'"
    $content = $content -replace "from '\.\.\/\.\.\/types\/notaFiscal'", "from '../../types'"
    $content = $content -replace "from '\.\./\.\./\.\./types/transporte'", "from '../../../types'"
    
    if($content -ne $original) {
        $content | Set-Content $file.FullName -NoNewline
        Write-Host "Updated: $($file.Name)"
    }
}

Write-Host "Done!"
