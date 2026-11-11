# NeuroJourney

NeuroJourney é um jogo educativo desenvolvido na Unity. Acompanhando Theo em diferentes lugares da vizinhança, o jogador conversa com personagens, identifica emoções e participa de atividades de memória. Cada história apresenta maneiras de reconhecer sentimentos e de lidar com situações do cotidiano.

## Visão geral

- **Gênero:** aventura narrativa e jogo da memória.
- **Tecnologia:** Unity, com projeto 2D e Universal Render Pipeline (URP).
- **Versão do Editor:** Unity `6000.5.8f1` (Unity 6).
- **Plataforma configurada:** há um perfil de build chamado `Web - Mobile - Development`. A plataforma final deve ser selecionada e validada no Unity antes da publicação.
- **Idioma do conteúdo:** português.

## Como abrir e executar

1. Instale a versão do Unity indicada acima pelo Unity Hub.
2. No Unity Hub, escolha **Add** ou **Open** e selecione a pasta raiz deste repositório, onde estão `Assets`, `Packages` e `ProjectSettings`.
3. Aguarde o Unity importar os arquivos e resolver os pacotes definidos em `Packages/manifest.json`.
4. Abra `Assets/Scenes/Menu.unity` e pressione **Play**.

Não é necessário instalar extensões do VS Code para executar o jogo. O Unity gerencia os pacotes do projeto por meio do Unity Package Manager.

## Fluxo do jogo

O fluxo principal passa pelo menu e pelo mapa. Cada local tem uma cena de entrada (`Antes`) com diálogo e identificação de emoção, seguida de uma atividade de memória e uma cena de encerramento (`Depois`). Ao concluir uma história, o progresso é salvo e o mapa apresenta o local restaurado.

| Cena | Função |
|---|---|
| `Menu` | Tela inicial do jogo. |
| `CasaTheo` | Cena relacionada à casa de Theo. |
| `Mapa` | Seleção dos locais e visualização das construções antes/depois da conclusão. |
| `PadariaAntes` / `PadariaDepois` | História da Dona Biscoito sobre tristeza e lembranças positivas. |
| `CorreiosAntes` / `CorreiosDepois` | História do Capitão Bigode sobre raiva e maneiras de se acalmar. |
| `ParqueAntes` / `ParqueDepois` | História de Luna sobre medo e formas de se sentir segura. |
| `PlazaAntes` / `PlazaDepois` | História de Zeca sobre alegria, empolgação e organização das brincadeiras. |

As cenas usadas na build principal são configuradas em `ProjectSettings/EditorBuildSettings.asset`. O projeto também contém cenas de exemplo de um pacote 2D; elas não fazem parte do fluxo principal do NeuroJourney.

## Controles e interação

- **Menus, diálogos e cartas:** interação por botões da interface, normalmente com clique ou toque.
- **Entrada nos locais:** `AbrirCena` permite abrir uma cena ao clicar no objeto; o script também detecta toque em dispositivos móveis.
- **Movimento do personagem:** `PlayerController` lê uma ação `MoveAction` do Input System. O mapeamento de teclas ou controles depende da configuração dessa ação no Inspector; o script não define teclas específicas.
- **Retorno ao mapa:** controlado pelo botão associado a `VoltarMapa`.

## Estrutura do repositório

| Pasta/arquivo | Responsabilidade |
|---|---|
| `Assets/Art/` | Sprites, ilustrações e arte do jogo, incluindo personagens e cenários. |
| `Assets/Scenes/` | Cenas Unity do menu, mapa, histórias e exemplos. |
| `Assets/Scripts/` | Lógica de interação, diálogos, memória, progresso, navegação e efeitos. |
| `Assets/Prefabs/` | Elementos reutilizáveis de interface e do jogo, como cartas, painéis e indicadores de progresso. |
| `Assets/Settings/` | Configurações e perfis de build da Unity. |
| `Assets/Materials/`, `Assets/Tiles/` e `Assets/Plugins/` | Materiais, recursos de tilemap e plugins utilizados pelo projeto. |
| `Assets/Text Mesh Pro/` | Recursos do TextMesh Pro para textos e exemplos associados ao pacote. |
| `Assets/_2DAdventureGame/` | Conteúdo e cenas de exemplo de um pacote 2D importado; não é o fluxo principal do jogo. |
| `Packages/manifest.json` | Lista e versões dos pacotes Unity necessários. |
| `Packages/packages-lock.json` | Resolução exata das dependências de pacotes. |
| `ProjectSettings/` | Configurações do projeto, incluindo versão do Editor, cenas de build e opções de renderização. |
| `Data/`, `FirstVersion/`, `FirstVersion2/` e `Version2/` | Arquivos de versões/builds Web publicados ou exportados. Para desenvolver o jogo, use as pastas `Assets`, `Packages` e `ProjectSettings`. |
| `NJ.icon/` | Recursos de ícone do projeto. |

## Responsabilidades dos scripts

Os scripts de `Assets/Scripts/` estão organizados por função:

### Navegação e personagem

| Script | O que controla |
|---|---|
| `AbrirCena.cs` | Abre a cena atribuída ao objeto quando ele recebe clique ou toque, ignorando interações sobre a interface. |
| `PlayerController.cs` | Move um objeto pela ação `MoveAction` configurada no Input System. |
| `VoltarMapa.cs` | Retorna ao mapa, usando a transição de nuvens quando disponível. |
| `VoltarMenu.cs` | Retorna ao menu e, na implementação atual, apaga o progresso das construções e figurinhas. |
| `GerenciadorTransicao.cs` | Implementa transições entre cenas com escurecimento e clareamento da tela. |
| `TransicaoNuvens.cs` | Implementa transição entre cenas com animação de nuvens. |

### Diálogos e fases

| Scripts | O que controlam |
|---|---|
| `DialogoPadariaAntes.cs` e `DialogoPadariaDepois.cs` | Conversas da Dona Biscoito, pergunta sobre emoção, feedback e conclusão da fase da padaria. |
| `DialogoCorreiosAntes.cs` e `DialogoCorreiosDepois.cs` | Conversas do Capitão Bigode e conclusão da fase dos Correios. |
| `DialogoParqueAntes.cs` e `DialogoParqueDepois.cs` | Conversas de Luna e conclusão da fase do parque. |
| `DialogoPlazaAntes.cs` e `DialogoPlazaDepois.cs` | Conversas de Zeca e conclusão da fase da praça. |
| `JogoMemoria.cs`, `JogoMemoriaCorreios.cs`, `JogoMemoriaParque.cs` e `JogoMemoriaPlaza.cs` | Criam e embaralham cartas, verificam pares, atualizam o progresso e iniciam a cena de encerramento correspondente. |
| `CartaMemoria.cs` | Exibe ou esconde uma carta e encaminha sua seleção ao jogo de memória ativo. |
| `ProgressoEstrelas.cs` | Atualiza as estrelas exibidas conforme os pares encontrados. |

### Progresso e colecionáveis

| Script | O que controla |
|---|---|
| `EstadoConstrucoes.cs` | Mostra no mapa cada construção em seu estado inicial ou restaurado, conforme os dados salvos. |
| `SistemaFigurinhas.cs` | Salva, consulta e apaga o estado desbloqueado das figurinhas usando `PlayerPrefs`. |
| `AnimacaoConquista.cs` | Desbloqueia uma figurinha e exibe a animação do painel de conquista. |
| `AlbumInterativo.cs` | Abre e fecha o álbum e verifica se o conjunto de figurinhas necessário para liberar o herói foi obtido. |
| `GerenciadorAlbum.cs` | Atualiza a exibição das figurinhas no álbum de acordo com o progresso salvo. |

### Efeitos e diagnóstico

| Script | O que controla |
|---|---|
| `AuraConstrucao.cs` | Cria e anima um brilho em torno de um sprite de construção. |
| `DiagnosticoPersonagem.cs` | Registra informações de diagnóstico de personagens no Console da Unity. |

## Progresso salvo

O jogo usa `PlayerPrefs` para persistir o estado das construções e das figurinhas entre sessões. As histórias registram as conclusões com as chaves `PadariaConcluida`, `CorreiosConcluido`, `ParqueConcluido` e `PlazaConcluida`; as figurinhas usam chaves com o prefixo `Figurinha_`.

> **Atenção:** no código atual, a ação de voltar ao menu (`VoltarMenu.cs`) limpa essas chaves. Portanto, esse botão reinicia o progresso salvo.

## Dependências principais

As dependências e suas versões ficam em `Packages/manifest.json`. O projeto utiliza, entre outros, o Universal Render Pipeline, o Input System, TextMesh Pro, ferramentas 2D e o Unity Test Framework. Ao abrir o projeto com a versão correta da Unity, o Package Manager restaura os pacotes declarados.

## Desenvolvimento

- Abra as cenas e scripts pelo Editor da Unity.
- Os campos públicos e serializados dos scripts devem ser associados aos objetos correspondentes no Inspector das cenas e prefabs.
- Para gerar uma build, use os perfis e opções de build do Editor da Unity e confira se as cenas necessárias estão incluídas. O perfil `Web - Mobile - Development` está em `Assets/Settings/Build Profiles/`.
- Não edite manualmente arquivos dentro de `Data/`, `FirstVersion/`, `FirstVersion2/` ou `Version2/` para alterar a lógica principal; eles são saídas/versões exportadas.

## Ajustes pendentes e onde fazer

O Unity Hub serve para instalar/abrir o projeto; as alterações abaixo são feitas no **Editor da Unity**, na janela **Project** e no **Inspector**. No Hub, abra o projeto com **Open** e selecione a pasta raiz deste repositório. Depois, use os caminhos abaixo na janela **Project**.

### 1. Cartas mudam de tamanho ao virar

Arquivos principais:

- `Assets/Prefabs/Carta.prefab`: dimensões e componentes visuais da carta.
- `Assets/Scripts/CartaMemoria.cs`: alternância entre a imagem da frente e o verso.
- A cena da fase e o componente de layout que contém a grade: controla o espaço reservado para cada carta.

No prefab, compare os valores de **Rect Transform** da raiz e das imagens da frente e do verso. Mantenha frente e verso ocupando a mesma área, e confira se a grade usa dimensões consistentes para os itens. Se a diferença vier da proporção dos sprites, ajuste **Preserve Aspect** no componente **Image** ou prepare imagens com proporções iguais. O script atual apenas ativa/desativa as imagens; ele não deveria redimensionar a carta. Teste o prefab em todas as fases de memória.

### 2. Botão de voltar ao menu não funciona

Arquivos principais:

- `Assets/Scenes/Mapa.unity`: cena onde o botão de retorno ao menu está configurado.
- `Assets/Scripts/VoltarMenu.cs`: método `Voltar()` que carrega a cena `Menu`.
- `ProjectSettings/EditorBuildSettings.asset`: lista de cenas disponíveis na build.

Abra `Mapa.unity`, selecione o botão e confira no **Inspector > Button > On Click()** se o evento aponta para um objeto que contém `VoltarMenu` e chama `Voltar()`. Verifique também se o botão está ativo e interagível, se há um **EventSystem** na cena e se a cena `Menu` está habilitada nas configurações de build. Execute pelo botão **Play** e confira erros na janela **Console**.

> **Importante:** atualmente `VoltarMenu.Voltar()` apaga o progresso das construções e figurinhas antes de carregar `Menu`. Se o retorno deve preservar o progresso, remova ou altere essa limpeza no script.

### 3. Ajustes de interface (UI)

Os elementos reutilizáveis ficam em `Assets/Prefabs/`, como `PainelDialogo.prefab`, `PainelPergunta.prefab`, `PainelFeedback.prefab`, `PainelMemoria.prefab`, `PainelMensagemPar.prefab` e `PainelConquista.prefab`. As posições específicas de cada fase ficam nas cenas correspondentes em `Assets/Scenes/`.

Para ajustar um elemento, abra o prefab ou a cena, selecione o objeto na hierarquia e use o **Inspector** para editar **Rect Transform**, âncoras, alinhamento, tamanho de fonte e componentes de imagem/texto. Confira a visualização em **Game** com diferentes proporções de tela; prefira âncoras e layout responsivos a posições fixas.

### 4. Adicionar música ou sons

1. Crie `Assets/Audio/` pela janela **Project** e importe arquivos de áudio licenciados para uso no projeto.
2. Na cena desejada, crie um objeto vazio pela hierarquia (**Create Empty**) e adicione o componente **Audio Source**.
3. Arraste a música para **AudioClip**; marque **Play On Awake** e **Loop** para música de fundo e ajuste **Volume**.
4. Para efeitos sonoros, use um **Audio Source** sem **Loop** e dispare a reprodução pela interação correspondente.

A configuração mais simples é adicionar a música às cenas necessárias; nesse caso, ela pode reiniciar a cada troca de cena. Para manter uma única reprodução contínua entre cenas, será necessário um gerenciador de áudio persistente, uma melhoria separada.
