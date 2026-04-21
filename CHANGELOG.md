# Changelog


## [v1.1.18-2](https://github.com/myarichuk/Json.Masker/compare/v1.1.14-2...v1.1.18-2) - 2026-04-21



### Build

- disable implicit static web assets in sample ([5adfb1b](https://github.com/myarichuk/Json.Masker/commit/5adfb1bf7d3695e7e82dfeade42a679e10f19c0a))
  
- **benchmarks:** suppress strong-name warnings # Conflicts: #	src/Json.Masker.Abstract/DefaultMaskingService.cs #	src/Json.Masker.Abstract/UtilExtensions.cs #	src/Json.Masker.AspNet.Newtonsoft/JsonMaskingMiddlewareExtension.cs #	src/Json.Masker.AspNet/JsonMaskingMiddlewareExtension.cs #	src/Json.Masker.Newtonsoft/MaskingValueProvider.cs #	src/Json.Masker.Newtonsoft/NewtonsoftMaskingExtensions.cs #	src/Json.Masker.SystemTextJson/MaskingEnumerableConverter.cs #	src/Json.Masker.SystemTextJson/MaskingScalarConverter.cs #	src/Json.Masker.SystemTextJson/MaskingStringDictionaryConverter.cs #	src/Json.Masker.SystemTextJson/TypeExtensions.cs ([e61002d](https://github.com/myarichuk/Json.Masker/commit/e61002d72b5d81c72307d66243c7120629153da3))
  
  
  
### CI

- fix gitversion configuration ([69cb55d](https://github.com/myarichuk/Json.Masker/commit/69cb55da6d50c8f43a1d0dd8d97ee52a75d4349d))
  
- install dotnet 8 in workflows ([47f6e4b](https://github.com/myarichuk/Json.Masker/commit/47f6e4b8877f17b3197d9b5234a9d0b4f526edb8))
  
  
  
  
### Documentation

- clarify masking defaults ([21026f2](https://github.com/myarichuk/Json.Masker/commit/21026f2bb2e69685cfcea3599922b2d192a08c30))
  
- expand plumbing guidance ([a1d131e](https://github.com/myarichuk/Json.Masker/commit/a1d131e25e75f8d0b5c6066e385932451a3d0cac))
  
  
  
### Features

- make default masking service more customizable ([1b1ffbe](https://github.com/myarichuk/Json.Masker/commit/1b1ffbebd4df6166aec032f676e846aa8c3c4b33))
  
- implement custom masking pattern support (DSL-like) ([163ccb7](https://github.com/myarichuk/Json.Masker/commit/163ccb7c98ff78c6e2b200fe5ee79d8189487e28))
  
- more masking patterns ([163b64f](https://github.com/myarichuk/Json.Masker/commit/163b64fdbcff3d20e35fd5f6d2c24f23f9af616d))
  
- implement basic functionality for both newtonsoft and system.text.json ([854f304](https://github.com/myarichuk/Json.Masker/commit/854f3048de0ee65953728e8b47712f262c814c03))
  
- **benchmarks:** expand masking comparisons ([725ef0a](https://github.com/myarichuk/Json.Masker/commit/725ef0a728bface42a9804781edf97c728d27c43))
  
  
  
### Fixes

- revert changelog generator template so it would properly work ([81765fd](https://github.com/myarichuk/Json.Masker/commit/81765fd5b00c9d17214ebe9ee96fcba4fa0179ed))
  
- revert changelog generator template so it would properly work ([76cd867](https://github.com/myarichuk/Json.Masker/commit/76cd8676918bc598dbc68ff05345126382628b97))
  
- literal handling in pattern masks ([1f94ce8](https://github.com/myarichuk/Json.Masker/commit/1f94ce84e680fe2a721b6358a717f0c2a2546f5d))
  
- solve merge conflict ([fce7f4c](https://github.com/myarichuk/Json.Masker/commit/fce7f4c8331526f3ec8fc6e3c4bcce4884e86cc1))
  
- restore writable masking service option ([00e2d78](https://github.com/myarichuk/Json.Masker/commit/00e2d782c405ed92dfd520b3da4a455983b0c73c))
  
-  missing parameter at reflection ctor ([6737dee](https://github.com/myarichuk/Json.Masker/commit/6737dee23dc476e5002f143e92b1b58b97afa3c9))
  
- make sure no irrelevant warnings during compilation ([a468c8a](https://github.com/myarichuk/Json.Masker/commit/a468c8a10a3f19fc0d5cb62b90932ca0ee53758a))
  
- **CI:** fix changelog tool paths (hopefully kek) ([b3b4ec2](https://github.com/myarichuk/Json.Masker/commit/b3b4ec296ae7116ad4a62b152abeb351bf159fb1))
  
- **CI:** another tentative (experiment) to make the friggin' changelog generation work ([70fb9c7](https://github.com/myarichuk/Json.Masker/commit/70fb9c7e15c06da873903ed59af6cdffbc6493d7))
  
- **CI:** hopefully last tweak of changelog and releases ([99d8b6b](https://github.com/myarichuk/Json.Masker/commit/99d8b6b2053d271f37d4ddf1aa50e235dadea4b8))
  
- **CI:** changelog template ([8d71214](https://github.com/myarichuk/Json.Masker/commit/8d71214114d88cdcbab804d1ae30980b2d9c707c))
  
- **CI:** adjust github action (fix release generation) and adjust changelog template ([f3d9f45](https://github.com/myarichuk/Json.Masker/commit/f3d9f453683ac0467020d9526b1df486420e0d27))
  
- **CI:** adjust changlog template ([c837ab8](https://github.com/myarichuk/Json.Masker/commit/c837ab8625a330943257c42c530552436967f3c5))
  
- **CI:** changelog related paths ([486c613](https://github.com/myarichuk/Json.Masker/commit/486c613ce228bf9f93eacb3a67cf422b887bee8f))
  
- **CI:** make sure github action that triggers on PR runs even if new commit is added ([a69c01a](https://github.com/myarichuk/Json.Masker/commit/a69c01a8307bae7d1b18eebf76a779eff39174a0))
  
- **CI:** fix git-chlog installation ([c506eba](https://github.com/myarichuk/Json.Masker/commit/c506ebabd9f2b2f18e073ce583a7f445e3eb125e))
  
- **CI:** fix github action yml syntax (facepalm) ([962e46f](https://github.com/myarichuk/Json.Masker/commit/962e46f97148bf4d1103c31baa6d7e562f35bcbd))
  
- **CI:** try to switch the github action that does changelog generation ([0204a02](https://github.com/myarichuk/Json.Masker/commit/0204a02ad6d3a2cdf74982917b7733b70dc83439))
  
- **CI:** unify tags and remove crap ([0ec7022](https://github.com/myarichuk/Json.Masker/commit/0ec702251dcbaf11550d5b91f83646ba366a75cd))
  
- **CI:** tentative fix - trying to supply version to changelog github action manually ([336dd8b](https://github.com/myarichuk/Json.Masker/commit/336dd8b87aca40db73dda64740e43cadb4e11bb4))
  
- **CI:** attempt to properly generate changelog ([ba55629](https://github.com/myarichuk/Json.Masker/commit/ba556296d0b83df8a54167efcb8ce3f01f653c7c))
  
- **CI:** remove unneeded pr labeler ([c335b49](https://github.com/myarichuk/Json.Masker/commit/c335b4936f932a08ccc3710343165ce7deac9aab))
  
- **CI:** try to finally make the changelog work! ([02d57e4](https://github.com/myarichuk/Json.Masker/commit/02d57e40bd4e7d0b3b3552af7b7460a4c9b9a50f))
  
- **ci:** now maybe publish pipeline would work (meh) ([cf23691](https://github.com/myarichuk/Json.Masker/commit/cf23691be8a74a2f52b6ce1790807b81c1a24fc1))
  
- **ci:** now properly fix changelog generation (stupid copy-paste error!) ([b939f77](https://github.com/myarichuk/Json.Masker/commit/b939f77035d58482ab4afff0ed2c39f976883b4b))
  
- **ci:** finally adjust all missing gitversion variables ([2f27e2c](https://github.com/myarichuk/Json.Masker/commit/2f27e2c48be9749629ae68c804973888b77d3df8))
  
- **ci:** adjust gitversion action output variable name ([d15df25](https://github.com/myarichuk/Json.Masker/commit/d15df25e6ea7a1f3c679470d6522176d86b054c6))
  
- **ci:** proper config for gitversion ([350c52f](https://github.com/myarichuk/Json.Masker/commit/350c52fbe24e6e9b51e0b789526ae87251d6a281))
  
- **ci:** try to fix gitversion versioning issue ([cdccd09](https://github.com/myarichuk/Json.Masker/commit/cdccd093c81238c75d504954a7898d90998a4f72))
  
- **ci:** use GitVersion 6.x ([c8d957a](https://github.com/myarichuk/Json.Masker/commit/c8d957abdb65050ab76772403efd3d605e387d41))
  
- **tests:** make sure we don't use url escaping encoder for tests for system.text.json ([95d5f17](https://github.com/myarichuk/Json.Masker/commit/95d5f17a31335730b3f6d72cdb73a399b0d4d7fb))
  
  
  
### Performance

- aggressively optimize JSON masking for performance and allocations ([3a9c981](https://github.com/myarichuk/Json.Masker/commit/3a9c981d4cc90eb0114b11947b69c68eb686912a))
  
- reduce allocations in scalar masking ([b0cd56d](https://github.com/myarichuk/Json.Masker/commit/b0cd56dc323b964796809fecd6908d9f44af35c2))
  
  
  
### Refactoring

- make improvements to newtonsoft masker, make some low level improvements and try to reduce allocations a bit more ([7023866](https://github.com/myarichuk/Json.Masker/commit/7023866505ec98997f4bc2a867ea0bc5e6252f22))
  
- some more optimization, shave off some latency of masking ([9bc94e7](https://github.com/myarichuk/Json.Masker/commit/9bc94e75c545a499e84e6a16721835bcef985e56))
  
- minimize allocations and refactor for better flow ([fcf73b7](https://github.com/myarichuk/Json.Masker/commit/fcf73b721104cfef059de3de6540c67ba402be47))
  
- showcase masking strategies in sample data ([124ba88](https://github.com/myarichuk/Json.Masker/commit/124ba880f909ce1214f0e01d7d94b254223b03c8))
  
- make default mask overridable ([4520b98](https://github.com/myarichuk/Json.Masker/commit/4520b98bb2fd87629f8c1a3d47ad6ee2851cc3b2))
  
- **benchmarks:** unify serialization scenarios ([56aa6f8](https://github.com/myarichuk/Json.Masker/commit/56aa6f844f3fed5b16aeeafd2ec35163a0e1723d))
  
  
  
### Style

- add xml docs and fix ordering in default masking service ([8ac72b2](https://github.com/myarichuk/Json.Masker/commit/8ac72b2c0c2f077baed7bd7438c9ae784538a37f))
  
  
  
### Tests

- more tests and fix pattern edge case ([a3a71f2](https://github.com/myarichuk/Json.Masker/commit/a3a71f2fb6b8f7bd69ef6d340f3cddc43ead6512))
  
- handle more edge cases - to be more specific, add tests for cases where pre-existing converters exist - they should be respected by maskers ([4f5b2d4](https://github.com/myarichuk/Json.Masker/commit/4f5b2d4abe9eb8e9f7d4a782c70e0e2e5ca0df7c))
  
- implement tests for custom pattern masking ([f4c4de6](https://github.com/myarichuk/Json.Masker/commit/f4c4de629fa324f16c49d43690a18d6a525791f7))
  
- implement basic tests to test happy paths ([b3b0611](https://github.com/myarichuk/Json.Masker/commit/b3b06115c3707bbf26639b1e44e7b8c486e3c1e3))
  
  
  
  




### Merges

- Merge pull request [#85](https://github.com/myarichuk/Json.Masker/issues/85) from myarichuk/jules-fix-branch-9626565017479977773
  
- Merge pull request [#71](https://github.com/myarichuk/Json.Masker/issues/71) from myarichuk/codex/add-github-actions-publishing-for-projects
  
- Merge pull request [#68](https://github.com/myarichuk/Json.Masker/issues/68) from myarichuk/codex/organize-sample-projects-and-improve-clarity
  
- Merge pull request [#66](https://github.com/myarichuk/Json.Masker/issues/66) from myarichuk/codex/add-xml-documentation-and-update-readme
  
- Merge pull request [#63](https://github.com/myarichuk/Json.Masker/issues/63) from myarichuk/refactor/optimize
  
- Merge pull request [#61](https://github.com/myarichuk/Json.Masker/issues/61) from myarichuk/chore/improve-benchmark-and-optimize
  
- Merge pull request [#56](https://github.com/myarichuk/Json.Masker/issues/56) from myarichuk/codex/add-libraries-to-benchmark-project
  
- Merge pull request [#55](https://github.com/myarichuk/Json.Masker/issues/55) from myarichuk/adjust-benchmark
  
- Merge pull request [#54](https://github.com/myarichuk/Json.Masker/issues/54) from myarichuk/codex/add-xml-docs-and-improve-benchmark-readability
  
- Merge pull request [#53](https://github.com/myarichuk/Json.Masker/issues/53) from myarichuk/codex/add-benchmarks-folder-and-implement-benchmark.net
  
- Merge pull request [#49](https://github.com/myarichuk/Json.Masker/issues/49) from myarichuk/feat/add-integration-sample
  
- Merge pull request [#47](https://github.com/myarichuk/Json.Masker/issues/47) from myarichuk/codex/add-xml-comments-and-fix-style-warnings
  
- Merge pull request [#44](https://github.com/myarichuk/Json.Masker/issues/44) from myarichuk/feat/more-flexible-masker
  
- Merge pull request [#43](https://github.com/myarichuk/Json.Masker/issues/43) from myarichuk/codex/suggest-missing-edge-cases-for-tests
  
- Merge pull request [#42](https://github.com/myarichuk/Json.Masker/issues/42) from myarichuk/codex/suggest-improvements-for-developer-story
  
- Merge pull request [#41](https://github.com/myarichuk/Json.Masker/issues/41) from myarichuk/codex/conduct-detailed-code-review
  
- Merge pull request [#40](https://github.com/myarichuk/Json.Masker/issues/40) from myarichuk/codex/add-xml-documentation-comments-and-tests
  
- Merge pull request [#39](https://github.com/myarichuk/Json.Masker/issues/39) from myarichuk/feat/custom-pattern
  
- Merge pull request [#38](https://github.com/myarichuk/Json.Masker/issues/38) from myarichuk/feat/more-mask-patterns
  
- Merge pull request [#37](https://github.com/myarichuk/Json.Masker/issues/37) from myarichuk/codex/add-xml-documentation-comments-for-public-items
  
- Merge pull request [#36](https://github.com/myarichuk/Json.Masker/issues/36) from myarichuk/codex/update-readme-with-examples-and-descriptions
  
- Merge pull request [#15](https://github.com/myarichuk/Json.Masker/issues/15) from myarichuk/dependabot/nuget/StyleCop.Analyzers.Unstable-1.2.0.556
  
- Merge pull request [#17](https://github.com/myarichuk/Json.Masker/issues/17) from myarichuk/dependabot/nuget/src/Json.Masker.Newtonsoft/Newtonsoft.Json-13.0.4
  
- Merge pull request [#20](https://github.com/myarichuk/Json.Masker/issues/20) from myarichuk/dependabot/nuget/tests/Json.Masker.Tests/xunit.runner.visualstudio-3.1.5
  
- Merge pull request [#21](https://github.com/myarichuk/Json.Masker/issues/21) from myarichuk/dependabot/nuget/tests/Json.Masker.Tests/Microsoft.NET.Test.Sdk-18.0.0
  
- Merge pull request [#27](https://github.com/myarichuk/Json.Masker/issues/27) from myarichuk/fix/try-to-make-things-generate-changelog
  
- Merge pull request [#25](https://github.com/myarichuk/Json.Masker/issues/25) from myarichuk/fix/try-to-make-things-generate-changelog
  
- Merge pull request [#24](https://github.com/myarichuk/Json.Masker/issues/24) from myarichuk/fix/try-to-make-things-generate-changelog
  
- Merge pull request [#23](https://github.com/myarichuk/Json.Masker/issues/23) from myarichuk/fix/try-to-make-things-generate-changelog
  
- Merge pull request [#22](https://github.com/myarichuk/Json.Masker/issues/22) from myarichuk/implement-basic-functionality
  
- Merge pull request [#14](https://github.com/myarichuk/Json.Masker/issues/14) from myarichuk/codex/evaluate-ci-pipeline-for-changelog-generation
  
- Merge pull request [#12](https://github.com/myarichuk/Json.Masker/issues/12) from myarichuk/fix/versioning-issue2
  
- Merge pull request [#11](https://github.com/myarichuk/Json.Masker/issues/11) from myarichuk/fix/versioning-issue
  
- Merge pull request [#10](https://github.com/myarichuk/Json.Masker/issues/10) from myarichuk/codex/fix-ci-build-issues-and-update-versioning
  
- Merge pull request [#9](https://github.com/myarichuk/Json.Masker/issues/9) from myarichuk/codex/fix-gitversion.tool-version-error
  
- Merge pull request [#8](https://github.com/myarichuk/Json.Masker/issues/8) from myarichuk/codex/refactor-project-structure-and-ci-setup
  
- Merge pull request [#1](https://github.com/myarichuk/Json.Masker/issues/1) from myarichuk/dependabot/github_actions/actions/checkout-5
  
- Merge pull request [#7](https://github.com/myarichuk/Json.Masker/issues/7) from myarichuk/codex/update-to-.net-8-in-global.json
  
- Merge pull request [#6](https://github.com/myarichuk/Json.Masker/issues/6) from myarichuk/codex/adjust-project-structure-for-multiple-json-libraries
  
- Merge pull request [#5](https://github.com/myarichuk/Json.Masker/issues/5) from myarichuk/codex/rename-placeholders-to-json.masker-and-michael-yarichuk
  
  


## [v1.1.14-2](https://github.com/myarichuk/Json.Masker/compare/v1.1.11-2...v1.1.14-2) - 2025-10-17



  
  




### Merges

- Merge pull request [#71](https://github.com/myarichuk/Json.Masker/issues/71) from myarichuk/codex/add-github-actions-publishing-for-projects
  
  


## [v1.1.11-2](https://github.com/myarichuk/Json.Masker/compare/v1.1.9-2...v1.1.11-2) - 2025-10-16



  
  






## [v1.1.9-2](https://github.com/myarichuk/Json.Masker/compare/v1.1.7-2...v1.1.9-2) - 2025-10-16



### Build

- **benchmarks:** suppress strong-name warnings # Conflicts: #	src/Json.Masker.Abstract/DefaultMaskingService.cs #	src/Json.Masker.Abstract/UtilExtensions.cs #	src/Json.Masker.AspNet.Newtonsoft/JsonMaskingMiddlewareExtension.cs #	src/Json.Masker.AspNet/JsonMaskingMiddlewareExtension.cs #	src/Json.Masker.Newtonsoft/MaskingValueProvider.cs #	src/Json.Masker.Newtonsoft/NewtonsoftMaskingExtensions.cs #	src/Json.Masker.SystemTextJson/MaskingEnumerableConverter.cs #	src/Json.Masker.SystemTextJson/MaskingScalarConverter.cs #	src/Json.Masker.SystemTextJson/MaskingStringDictionaryConverter.cs #	src/Json.Masker.SystemTextJson/TypeExtensions.cs ([67f6bae](https://github.com/myarichuk/Json.Masker/commit/67f6bae0abea92433f00eb10d204525451042ccb))
  
  
  
  
  




### Merges

- Merge pull request [#68](https://github.com/myarichuk/Json.Masker/issues/68) from myarichuk/codex/organize-sample-projects-and-improve-clarity
  
  


## [v1.1.7-2](https://github.com/myarichuk/Json.Masker/compare/v1.1.5-2...v1.1.7-2) - 2025-10-16



  
  






## [v1.1.5-2](https://github.com/myarichuk/Json.Masker/compare/v1.1.3-8...v1.1.5-2) - 2025-10-16



  
  




### Merges

- Merge pull request [#66](https://github.com/myarichuk/Json.Masker/issues/66) from myarichuk/codex/add-xml-documentation-and-update-readme
  
  


## [v1.1.3-8](https://github.com/myarichuk/Json.Masker/compare/v1.1.1-3...v1.1.3-8) - 2025-10-16



  
### Refactoring

- make improvements to newtonsoft masker, make some low level improvements and try to reduce allocations a bit more ([36aff67](https://github.com/myarichuk/Json.Masker/commit/36aff67cec7ab3c1aee52061c055a6e9b57a23d8))
  
- some more optimization, shave off some latency of masking ([b92e8ef](https://github.com/myarichuk/Json.Masker/commit/b92e8ef367c6b8c708d67576028e14eda6751613))
  
- minimize allocations and refactor for better flow ([035c556](https://github.com/myarichuk/Json.Masker/commit/035c556a5aa8eae38cd70dc94b096f80eb339ed4))
  
- **benchmarks:** unify serialization scenarios ([e8cac39](https://github.com/myarichuk/Json.Masker/commit/e8cac39e0869d8afc5c67f44f0baaf8e8ffeeca0))
  
  
  
  




### Merges

- Merge pull request [#63](https://github.com/myarichuk/Json.Masker/issues/63) from myarichuk/refactor/optimize
  
  


## [v1.1.1-3](https://github.com/myarichuk/Json.Masker/compare/v1.0.7-2...v1.1.1-3) - 2025-10-13



  
### Features

- **benchmarks:** expand masking comparisons ([bab50b5](https://github.com/myarichuk/Json.Masker/commit/bab50b53f352e55237920d611c9645b3b1b655a1))
  
  
  
  




### Merges

- Merge pull request [#61](https://github.com/myarichuk/Json.Masker/issues/61) from myarichuk/chore/improve-benchmark-and-optimize
  
- Merge pull request [#56](https://github.com/myarichuk/Json.Masker/issues/56) from myarichuk/codex/add-libraries-to-benchmark-project
  
  


## [v1.0.7-2](https://github.com/myarichuk/Json.Masker/compare/v1.0.5-2...v1.0.7-2) - 2025-10-13



  
  




### Merges

- Merge pull request [#55](https://github.com/myarichuk/Json.Masker/issues/55) from myarichuk/adjust-benchmark
  
  


## [v1.0.5-2](https://github.com/myarichuk/Json.Masker/compare/v1.0.3-21...v1.0.5-2) - 2025-10-12



  
  




### Merges

- Merge pull request [#54](https://github.com/myarichuk/Json.Masker/issues/54) from myarichuk/codex/add-xml-docs-and-improve-benchmark-readability
  
  


## [v1.0.3-21](https://github.com/myarichuk/Json.Masker/compare/v1.0.0-17...v1.0.3-21) - 2025-10-12



  
  




### Merges

- Merge pull request [#53](https://github.com/myarichuk/Json.Masker/issues/53) from myarichuk/codex/add-benchmarks-folder-and-implement-benchmark.net
  
  


## [v1.0.0-17](https://github.com/myarichuk/Json.Masker/compare/v0.5.5-2...v1.0.0-17) - 2025-10-12



### Build

- disable implicit static web assets in sample ([52a4f83](https://github.com/myarichuk/Json.Masker/commit/52a4f83269aad0796b8f2818f68a4147a9893fb2))
  
  
  
  
### Fixes

- revert changelog generator template so it would properly work ([7131f88](https://github.com/myarichuk/Json.Masker/commit/7131f88303ac8b964a5b8c1f264a4e9451ff17eb))
  
  
  
### Performance

- reduce allocations in scalar masking ([de6e67f](https://github.com/myarichuk/Json.Masker/commit/de6e67f4bed6974834cb7362f03e91ca7ea62a6a))
  
  
  
### Refactoring

- showcase masking strategies in sample data ([891b09c](https://github.com/myarichuk/Json.Masker/commit/891b09cec00f3f1dbd5a2953176b50a29576edce))
  
  
  
  




### Merges

- Merge pull request [#49](https://github.com/myarichuk/Json.Masker/issues/49) from myarichuk/feat/add-integration-sample
  
  


## [v0.5.5-2](https://github.com/myarichuk/Json.Masker/compare/v0.5.3-2...v0.5.5-2) - 2025-10-10



  
### Style

- add xml docs and fix ordering in default masking service ([f7aa3b7](https://github.com/myarichuk/Json.Masker/commit/f7aa3b7b150b5128148f2c8fd64fb6a00ad49e5a))
  
  
  
  




### Merges

- Merge pull request [#47](https://github.com/myarichuk/Json.Masker/issues/47) from myarichuk/codex/add-xml-comments-and-fix-style-warnings
  
  


## [v0.5.3-2](https://github.com/myarichuk/Json.Masker/compare/v0.5.1-2...v0.5.3-2) - 2025-10-10



  
### Fixes

- revert changelog generator template so it would properly work ([a055c40](https://github.com/myarichuk/Json.Masker/commit/a055c406eeae7d62090424789b36adaf839913bd))
  
  
  
  






## [v0.5.1-2](https://github.com/myarichuk/Json.Masker/compare/v0.5.0-5...v0.5.1-2) - 2025-10-10



  
  






## [v0.5.0-5](https://github.com/myarichuk/Json.Masker/compare/v0.4.10-5...v0.5.0-5) - 2025-10-10



  
### Features

- make default masking service more customizable ([cc6249a](https://github.com/myarichuk/Json.Masker/commit/cc6249a7495f4ddd81409c79abd1806a8d9bb844))
  
  
  
### Fixes

- literal handling in pattern masks ([18468d6](https://github.com/myarichuk/Json.Masker/commit/18468d65355d1fa3697cda63fc5d6896cf789f6f))
  
  
  
### Tests

- more tests and fix pattern edge case ([0d1795c](https://github.com/myarichuk/Json.Masker/commit/0d1795c0e82865acebb68230f940fee955cf69c5))
  
  
  
  




### Merges

- Merge pull request [#44](https://github.com/myarichuk/Json.Masker/issues/44) from myarichuk/feat/more-flexible-masker
  
  


## [v0.4.10-5](https://github.com/myarichuk/Json.Masker/compare/v0.4.6-2...v0.4.10-5) - 2025-10-10



  
### Fixes

- solve merge conflict ([f9129f5](https://github.com/myarichuk/Json.Masker/commit/f9129f523f024b367a317e447b5cd6b797704e86))
  
  
  
### Tests

- handle more edge cases - to be more specific, add tests for cases where pre-existing converters exist - they should be respected by maskers ([d43de1d](https://github.com/myarichuk/Json.Masker/commit/d43de1d8d8b706119877c61166bc37687039d5a0))
  
  
  
  




### Merges

- Merge pull request [#43](https://github.com/myarichuk/Json.Masker/issues/43) from myarichuk/codex/suggest-missing-edge-cases-for-tests
  
  


## [v0.4.6-2](https://github.com/myarichuk/Json.Masker/compare/v0.4.4-2...v0.4.6-2) - 2025-10-10



  
### Refactoring

- make default mask overridable ([fef09a8](https://github.com/myarichuk/Json.Masker/commit/fef09a845bc49e7a6960918c76be8efc76090585))
  
  
  
  




### Merges

- Merge pull request [#42](https://github.com/myarichuk/Json.Masker/issues/42) from myarichuk/codex/suggest-improvements-for-developer-story
  
  


## [v0.4.4-2](https://github.com/myarichuk/Json.Masker/compare/v0.4.2-2...v0.4.4-2) - 2025-10-10



  
### Fixes

- restore writable masking service option ([28dba14](https://github.com/myarichuk/Json.Masker/commit/28dba140a53612284baede591bf3707f274c7d49))
  
  
  
  




### Merges

- Merge pull request [#41](https://github.com/myarichuk/Json.Masker/issues/41) from myarichuk/codex/conduct-detailed-code-review
  
  


## [v0.4.2-2](https://github.com/myarichuk/Json.Masker/compare/v0.4.0-7...v0.4.2-2) - 2025-10-10



  
### Documentation

- clarify masking defaults ([94a087a](https://github.com/myarichuk/Json.Masker/commit/94a087a2a93b38e9cde2c6da8520cf1bcd6410e0))
  
  
  
  




### Merges

- Merge pull request [#40](https://github.com/myarichuk/Json.Masker/issues/40) from myarichuk/codex/add-xml-documentation-comments-and-tests
  
  


## [v0.4.0-7](https://github.com/myarichuk/Json.Masker/compare/v0.3.0-2...v0.4.0-7) - 2025-10-10



  
### Features

- implement custom masking pattern support (DSL-like) ([1742a34](https://github.com/myarichuk/Json.Masker/commit/1742a34f0bc554e527802460d48bc748ea4850fb))
  
  
  
### Fixes

-  missing parameter at reflection ctor ([7acf54a](https://github.com/myarichuk/Json.Masker/commit/7acf54a0dbe2dc97042efa87789d2a634bcae314))
  
- **CI:** remove unneeded pr labeler ([f6330b1](https://github.com/myarichuk/Json.Masker/commit/f6330b15ed00f95c8b685431c99bd7b4f9ec5280))
  
- **CI:** make sure github action that triggers on PR runs even if new commit is added ([81331a7](https://github.com/myarichuk/Json.Masker/commit/81331a7891091765446808a952eeeaaec6268b9d))
  
  
  
### Tests

- implement tests for custom pattern masking ([cb6470a](https://github.com/myarichuk/Json.Masker/commit/cb6470a13eea02f68ce8d7f82dce1d6c9cd31490))
  
  
  
  




### Merges

- Merge pull request [#39](https://github.com/myarichuk/Json.Masker/issues/39) from myarichuk/feat/custom-pattern
  
  


## [v0.3.0-2](https://github.com/myarichuk/Json.Masker/compare/v0.2.26-2...v0.3.0-2) - 2025-10-09



### Features

- more masking patterns ([f70a22a](https://github.com/myarichuk/Json.Masker/commit/f70a22a2473f3823b33c59bdb9e65e4042b31e41))
  
  
  
  




### Merges

- Merge pull request [#38](https://github.com/myarichuk/Json.Masker/issues/38) from myarichuk/feat/more-mask-patterns
  
  


## [v0.2.26-2](https://github.com/myarichuk/Json.Masker/compare/v0.2.22-2...v0.2.26-2) - 2025-10-09



  
  




### Merges

- Merge pull request [#37](https://github.com/myarichuk/Json.Masker/issues/37) from myarichuk/codex/add-xml-documentation-comments-for-public-items
  
  


## [v0.2.22-2](https://github.com/myarichuk/Json.Masker/compare/v0.2.20-2...v0.2.22-2) - 2025-10-09



  
### Documentation

- expand plumbing guidance ([16b3841](https://github.com/myarichuk/Json.Masker/commit/16b38413bf07a19a92834465406e07a53cb69635))
  
  
  
  




### Merges

- Merge pull request [#36](https://github.com/myarichuk/Json.Masker/issues/36) from myarichuk/codex/update-readme-with-examples-and-descriptions
  
  


## [v0.2.20-2](https://github.com/myarichuk/Json.Masker/compare/v0.2.2-2...v0.2.20-2) - 2025-10-09



  
### Fixes

- **CI:** unify tags and remove crap ([a7461ab](https://github.com/myarichuk/Json.Masker/commit/a7461abe96f6ca178f4c7a06e6dfcdec0feb602a))
  
- **CI:** hopefully last tweak of changelog and releases ([05b1064](https://github.com/myarichuk/Json.Masker/commit/05b1064074e97ce9592716f3f47451e327102090))
  
- **CI:** changelog template ([483cbde](https://github.com/myarichuk/Json.Masker/commit/483cbdeead92283e6f9877a3d1e0174845b43de1))
  
- **CI:** adjust github action (fix release generation) and adjust changelog template ([d1171ad](https://github.com/myarichuk/Json.Masker/commit/d1171ad435d1e4d115e3bfc9a53e1e85d6091104))
  
- **CI:** adjust changlog template ([04fb68d](https://github.com/myarichuk/Json.Masker/commit/04fb68df6de913e881d2470ab085c957d6fae6c4))
  
- **CI:** changelog related paths ([afe00af](https://github.com/myarichuk/Json.Masker/commit/afe00af25030c5d0ed2c7ece07afd3429e21dc4c))
  
- **CI:** fix changelog tool paths (hopefully kek) ([79b7df2](https://github.com/myarichuk/Json.Masker/commit/79b7df230c7965a9ed70a3c3e61aa832886fc619))
  
- **CI:** fix git-chlog installation ([9889a08](https://github.com/myarichuk/Json.Masker/commit/9889a08e3b1c57e21660dedfbb1c93505fabc5aa))
  
- **CI:** fix github action yml syntax (facepalm) ([e87dcff](https://github.com/myarichuk/Json.Masker/commit/e87dcff8aed546a2d2fc4e90b374c21b721ab8a6))
  
- **CI:** try to switch the github action that does changelog generation ([fdb7275](https://github.com/myarichuk/Json.Masker/commit/fdb7275258b01f4705d43e0454349c068e73336b))
  
- **CI:** another tentative (experiment) to make the friggin' changelog generation work ([237c3f3](https://github.com/myarichuk/Json.Masker/commit/237c3f3f0c4b8915c2ca6bcaa2e446a774e384fe))
  
  
  
  




### Merges

- Merge pull request [#15](https://github.com/myarichuk/Json.Masker/issues/15) from myarichuk/dependabot/nuget/StyleCop.Analyzers.Unstable-1.2.0.556
  
- Merge pull request [#17](https://github.com/myarichuk/Json.Masker/issues/17) from myarichuk/dependabot/nuget/src/Json.Masker.Newtonsoft/Newtonsoft.Json-13.0.4
  
- Merge pull request [#20](https://github.com/myarichuk/Json.Masker/issues/20) from myarichuk/dependabot/nuget/tests/Json.Masker.Tests/xunit.runner.visualstudio-3.1.5
  
- Merge pull request [#21](https://github.com/myarichuk/Json.Masker/issues/21) from myarichuk/dependabot/nuget/tests/Json.Masker.Tests/Microsoft.NET.Test.Sdk-18.0.0
  
- Merge pull request [#27](https://github.com/myarichuk/Json.Masker/issues/27) from myarichuk/fix/try-to-make-things-generate-changelog
  
- Merge pull request [#25](https://github.com/myarichuk/Json.Masker/issues/25) from myarichuk/fix/try-to-make-things-generate-changelog
  
  


## [v0.2.2-2](https://github.com/myarichuk/Json.Masker/compare/v0.2.1-2...v0.2.2-2) - 2025-10-09



  
### Fixes

- **CI:** tentative fix - trying to supply version to changelog github action manually ([03e1373](https://github.com/myarichuk/Json.Masker/commit/03e1373f418941039ebff7b7643b576b7304a611))
  
  
  
  




### Merges

- Merge pull request [#24](https://github.com/myarichuk/Json.Masker/issues/24) from myarichuk/fix/try-to-make-things-generate-changelog
  
  


## [v0.2.1-2](https://github.com/myarichuk/Json.Masker/compare/v0.2.0-6...v0.2.1-2) - 2025-10-08



  
### Fixes

- **CI:** attempt to properly generate changelog ([91c9c0a](https://github.com/myarichuk/Json.Masker/commit/91c9c0a35069884b3f415bc258fe65d57f606f10))
  
  
  
  




### Merges

- Merge pull request [#23](https://github.com/myarichuk/Json.Masker/issues/23) from myarichuk/fix/try-to-make-things-generate-changelog
  
  


## [v0.2.0-6](https://github.com/myarichuk/Json.Masker/compare/v0.1.5-1...v0.2.0-6) - 2025-10-08



  
### Features

- implement basic functionality for both newtonsoft and system.text.json ([a01b5ca](https://github.com/myarichuk/Json.Masker/commit/a01b5ca6e0a1a6eadd7f7bb0cab156563d92a825))
  
  
  
### Fixes

- **CI:** try to finally make the changelog work! ([be94c14](https://github.com/myarichuk/Json.Masker/commit/be94c14b678a71f40a2b92c59e9760e8150df552))
  
- **tests:** make sure we don't use url escaping encoder for tests for system.text.json ([a894fba](https://github.com/myarichuk/Json.Masker/commit/a894fba4cee813bb2b600646ad5d442397e51504))
  
  
  
### Tests

- implement basic tests to test happy paths ([b552c80](https://github.com/myarichuk/Json.Masker/commit/b552c80384c1b0ee92a63b8d420be4a6b0143e28))
  
  
  
  




### Merges

- Merge pull request [#22](https://github.com/myarichuk/Json.Masker/issues/22) from myarichuk/implement-basic-functionality
  
  


## [v0.1.5-1](https://github.com/myarichuk/Json.Masker/compare/v0.1.4-2...v0.1.5-1) - 2025-09-25



  
  






## [v0.1.4-2](https://github.com/myarichuk/Json.Masker/compare/v0.1.2.0...v0.1.4-2) - 2025-09-25



  
  




### Merges

- Merge pull request [#14](https://github.com/myarichuk/Json.Masker/issues/14) from myarichuk/codex/evaluate-ci-pipeline-for-changelog-generation
  
  


## [v0.1.2.0](https://github.com/myarichuk/Json.Masker/compare/v0.1.1.0...v0.1.2.0) - 2025-09-25



  
  






## [v0.1.1.0](https://github.com/myarichuk/Json.Masker/compare/v0.0.17.0...v0.1.1.0) - 2025-09-25



  
### Fixes

- **ci:** now maybe publish pipeline would work (meh) ([a56694a](https://github.com/myarichuk/Json.Masker/commit/a56694ad63336cd1577fd8e6396ec3b6ce2f9129))
  
- **ci:** now properly fix changelog generation (stupid copy-paste error!) ([66003fe](https://github.com/myarichuk/Json.Masker/commit/66003febc144a30292c3f1fdb00e4aed9704494b))
  
  
  
  






## [v0.0.17.0](https://github.com/myarichuk/Json.Masker/compare/v0.0.16.0...v0.0.17.0) - 2025-09-24



  
### Fixes

- make sure no irrelevant warnings during compilation ([620d17b](https://github.com/myarichuk/Json.Masker/commit/620d17bd7ec1705d038a99ff6151d91e97363b41))
  
  
  
  






## [v0.0.16.0](https://github.com/myarichuk/Json.Masker/compare/v0.0.14.0...v0.0.16.0) - 2025-09-24



  
  






## v0.0.14.0 - 2025-09-24



### CI

- fix gitversion configuration ([6812bf6](https://github.com/myarichuk/Json.Masker/commit/6812bf6b7543f268dc89b81c4bbe3a306eef03e0))
  
- install dotnet 8 in workflows ([c5b7945](https://github.com/myarichuk/Json.Masker/commit/c5b7945cc0535e6b48d50996c9a9bbb50e1d5bb6))
  
  
  
  
### Fixes

- **ci:** finally adjust all missing gitversion variables ([2f22f1a](https://github.com/myarichuk/Json.Masker/commit/2f22f1a191fca01dd6542c36053fb0bdb4b6c3a6))
  
- **ci:** adjust gitversion action output variable name ([878403d](https://github.com/myarichuk/Json.Masker/commit/878403dae5dc2c73049c2ba1d9b2f0644abfdf7e))
  
- **ci:** proper config for gitversion ([e0b9845](https://github.com/myarichuk/Json.Masker/commit/e0b984512317bda2571cef353310ac2708f10298))
  
- **ci:** try to fix gitversion versioning issue ([8cdd8ed](https://github.com/myarichuk/Json.Masker/commit/8cdd8ed7c13d7e5e1a0664b528bdd20a78344415))
  
- **ci:** use GitVersion 6.x ([8dfb8c6](https://github.com/myarichuk/Json.Masker/commit/8dfb8c69738a7378cb77ab895e25cc514d165314))
  
  
  
  




### Merges

- Merge pull request [#12](https://github.com/myarichuk/Json.Masker/issues/12) from myarichuk/fix/versioning-issue2
  
- Merge pull request [#11](https://github.com/myarichuk/Json.Masker/issues/11) from myarichuk/fix/versioning-issue
  
- Merge pull request [#10](https://github.com/myarichuk/Json.Masker/issues/10) from myarichuk/codex/fix-ci-build-issues-and-update-versioning
  
- Merge pull request [#9](https://github.com/myarichuk/Json.Masker/issues/9) from myarichuk/codex/fix-gitversion.tool-version-error
  
- Merge pull request [#8](https://github.com/myarichuk/Json.Masker/issues/8) from myarichuk/codex/refactor-project-structure-and-ci-setup
  
- Merge pull request [#1](https://github.com/myarichuk/Json.Masker/issues/1) from myarichuk/dependabot/github_actions/actions/checkout-5
  
- Merge pull request [#7](https://github.com/myarichuk/Json.Masker/issues/7) from myarichuk/codex/update-to-.net-8-in-global.json
  
- Merge pull request [#6](https://github.com/myarichuk/Json.Masker/issues/6) from myarichuk/codex/adjust-project-structure-for-multiple-json-libraries
  
- Merge pull request [#5](https://github.com/myarichuk/Json.Masker/issues/5) from myarichuk/codex/rename-placeholders-to-json.masker-and-michael-yarichuk
  
  


