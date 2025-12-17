# Script para reorganizar o formulário de produto
$filePath = "src\pages\Produtos\ProdutoFormPage.tsx"

# Ler o conteúdo do arquivo
$content = Get-Content -Path $filePath -Raw -Encoding UTF8

# Texto a ser substituído (da linha 1170 até aproximadamente 1295)
$oldText = @"
        {/* DescriÇőÇo - Campo Principal - Igual VB6 */}
        <div className="bg-white rounded-2xl border border-gray-100 shadow-sm p-5">
          {/* CabeÇőalho com DescriÇőÇo e Status agrupados */}
          <div className="grid gap-4 lg:grid-cols-3 mb-4">
            <div className="lg:col-span-2">
              <InputModerno
                label="DescriÇőÇo"
                value={formData.descricao}
                onChange={(v) => handleChange('descricao', v)}
                required
                disabled={isViewMode}
                icone={<Package className="w-4 h-4" />}
              />
            </div>

            <div className="bg-gray-50 border border-gray-100 rounded-xl p-3">
              <div className="flex items-center justify-between gap-2 text-xs font-semibold text-gray-600 uppercase tracking-wide">
                <span>Status e controle</span>
                <CheckCircle2 className="w-4 h-4 text-gray-400" />
              </div>
              <div className="flex flex-wrap gap-2 mt-3">
                <CheckboxChip
                  checked={formData.inativo}
                  onChange={(v) => handleChange('inativo', v)}
                  label="Inativo"
                  cor="red"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.obsoleto}
                  onChange={(v) => handleChange('obsoleto', v)}
                  label="Obsoleto"
                  cor="amber"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.travaReceita}
                  onChange={(v) => handleChange('travaReceita', v)}
                  label="Trava Receita"
                  cor="red"
                  disabled={isViewMode}
                />
              </div>
              <div className="flex flex-wrap gap-2 mt-2">
                <CheckboxChip
                  checked={formData.lance}
                  onChange={(v) => handleChange('lance', v)}
                  label="Lance"
                  cor="blue"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.eRegulador}
                  onChange={(v) => handleChange('eRegulador', v)}
                  label="E Regulador"
                  cor="orange"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.mpInicial}
                  onChange={(v) => handleChange('mpInicial', v)}
                  label="M.Prima Inicial"
                  cor="green"
                  disabled={isViewMode}
                />
              </div>
            </div>
          </div>

          {/* Agrupamento das caracterÇđsticas */}
          <div className="grid gap-4 border-t border-gray-100 pt-4 lg:grid-cols-2">
            <div className="bg-gray-50 border border-gray-100 rounded-xl p-3">
              <p className="text-xs font-semibold text-gray-600 uppercase tracking-wide">Caracteristicas</p>
              <div className="flex flex-wrap gap-2 mt-2">
                <CheckboxChip
                  checked={formData.eMateriaPrima}
                  onChange={(v) => handleChange('eMateriaPrima', v)}
                  label="MatÇ¸ria Prima"
                  cor="purple"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.materialAdquiridoDeTerceiro}
                  onChange={(v) => handleChange('materialAdquiridoDeTerceiro', v)}
                  label="Material Ad. de Terceiro"
                  cor="blue"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.industrializacao}
                  onChange={(v) => handleChange('industrializacao', v)}
                  label="IndustrializaÇőÇo"
                  cor="purple"
                  disabled={isViewMode}
                />
              </div>
            </div>
            <div className="bg-gray-50 border border-gray-100 rounded-xl p-3">
              <p className="text-xs font-semibold text-gray-600 uppercase tracking-wide">Uso e origem</p>
              <div className="flex flex-wrap gap-2 mt-2">
                <CheckboxChip
                  checked={formData.usado}
                  onChange={(v) => handleChange('usado', v)}
                  label="Usado"
                  cor="amber"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.sucata}
                  onChange={(v) => handleChange('sucata', v)}
                  label="Sucata"
                  cor="amber"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.importado}
                  onChange={(v) => handleChange('importado', v)}
                  label="Importado"
                  cor="green"
                  disabled={isViewMode}
                />
              </div>
            </div>
          </div>
        </div>
"@

$newText = @"
        {/* Descrição - Campo Principal */}
        <div className="bg-white rounded-2xl border border-gray-100 shadow-sm p-5">
          <InputModerno
            label="Descrição"
            value={formData.descricao}
            onChange={(v) => handleChange('descricao', v)}
            required
            disabled={isViewMode}
            icone={<Package className="w-4 h-4" />}
          />
        </div>

        {/* Status do Produto */}
        <SecaoCard 
          titulo="Status do Produto" 
          subtitulo="Situação atual e disponibilidade" 
          icone={<CheckCircle2 className="w-5 h-5" />}
        >
          <div className="flex flex-wrap gap-3">
            <CheckboxChip
              checked={formData.inativo}
              onChange={(v) => handleChange('inativo', v)}
              label="Inativo"
              cor="red"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.obsoleto}
              onChange={(v) => handleChange('obsoleto', v)}
              label="Obsoleto"
              cor="amber"
              disabled={isViewMode}
            />
            <CheckboxChip
              checked={formData.travaReceita}
              onChange={(v) => handleChange('travaReceita', v)}
              label="Trava Receita"
              cor="red"
              disabled={isViewMode}
            />
          </div>
        </SecaoCard>

        {/* Características do Produto */}
        <SecaoCard 
          titulo="Características do Produto" 
          subtitulo="Classificação e tipo de material" 
          icone={<Box className="w-5 h-5" />}
        >
          <div className="space-y-4">
            {/* Tipo de Material */}
            <div>
              <p className="text-xs font-semibold text-gray-600 uppercase tracking-wide mb-3">Tipo de Material</p>
              <div className="flex flex-wrap gap-2">
                <CheckboxChip
                  checked={formData.eMateriaPrima}
                  onChange={(v) => handleChange('eMateriaPrima', v)}
                  label="Matéria Prima"
                  cor="purple"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.materialAdquiridoDeTerceiro}
                  onChange={(v) => handleChange('materialAdquiridoDeTerceiro', v)}
                  label="Material Ad. de Terceiro"
                  cor="blue"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.mpInicial}
                  onChange={(v) => handleChange('mpInicial', v)}
                  label="M.Prima Inicial"
                  cor="green"
                  disabled={isViewMode}
                />
              </div>
            </div>

            {/* Processamento */}
            <div className="pt-3 border-t border-gray-100">
              <p className="text-xs font-semibold text-gray-600 uppercase tracking-wide mb-3">Processamento</p>
              <div className="flex flex-wrap gap-2">
                <CheckboxChip
                  checked={formData.industrializacao}
                  onChange={(v) => handleChange('industrializacao', v)}
                  label="Industrialização"
                  cor="purple"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.lance}
                  onChange={(v) => handleChange('lance', v)}
                  label="Lance"
                  cor="blue"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.eRegulador}
                  onChange={(v) => handleChange('eRegulador', v)}
                  label="É Regulador"
                  cor="orange"
                  disabled={isViewMode}
                />
              </div>
            </div>

            {/* Condição e Origem */}
            <div className="pt-3 border-t border-gray-100">
              <p className="text-xs font-semibold text-gray-600 uppercase tracking-wide mb-3">Condição e Origem</p>
              <div className="flex flex-wrap gap-2">
                <CheckboxChip
                  checked={formData.usado}
                  onChange={(v) => handleChange('usado', v)}
                  label="Usado"
                  cor="amber"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.sucata}
                  onChange={(v) => handleChange('sucata', v)}
                  label="Sucata"
                  cor="amber"
                  disabled={isViewMode}
                />
                <CheckboxChip
                  checked={formData.importado}
                  onChange={(v) => handleChange('importado', v)}
                  label="Importado"
                  cor="green"
                  disabled={isViewMode}
                />
              </div>
            </div>
          </div>
        </SecaoCard>
"@

# Substituir o texto
$content = $content -replace [regex]::Escape($oldText), $newText

# Salvar o arquivo
Set-Content -Path $filePath -Value $content -Encoding UTF8 -NoNewline

Write-Host "Arquivo atualizado com sucesso!" -ForegroundColor Green
