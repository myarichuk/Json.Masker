# Changelog

All notable changes to this project will be documented in this file.


## [v0.2.2-2](https://github.com/myarichuk/Json.Masker/compare/v0.2.1-2...v0.2.2-2) - 2025-10-09



### Bug Fixes

- **CI:** tentative fix - trying to supply version to changelog github action manually ([03e1373](https://github.com/myarichuk/Json.Masker/commit/03e1373f418941039ebff7b7643b576b7304a611))
  
  
  
  
  




### Merges

- Merge pull request [#24](https://github.com/myarichuk/Json.Masker/issues/24) from myarichuk/fix/try-to-make-things-generate-changelog
  
  


## [v0.2.1-2](https://github.com/myarichuk/Json.Masker/compare/v0.2.0-6...v0.2.1-2) - 2025-10-08



### Bug Fixes

- **CI:** attempt to properly generate changelog ([91c9c0a](https://github.com/myarichuk/Json.Masker/commit/91c9c0a35069884b3f415bc258fe65d57f606f10))
  
  
  
  
  




### Merges

- Merge pull request [#23](https://github.com/myarichuk/Json.Masker/issues/23) from myarichuk/fix/try-to-make-things-generate-changelog
  
  


## [v0.2.0-6](https://github.com/myarichuk/Json.Masker/compare/v0.1.5-1...v0.2.0-6) - 2025-10-08



### Bug Fixes

- **CI:** try to finally make the changelog work! ([be94c14](https://github.com/myarichuk/Json.Masker/commit/be94c14b678a71f40a2b92c59e9760e8150df552))
  
- **tests:** make sure we don't use url escaping encoder for tests for system.text.json ([a894fba](https://github.com/myarichuk/Json.Masker/commit/a894fba4cee813bb2b600646ad5d442397e51504))
  
  
  
  
### Features

- implement basic functionality for both newtonsoft and system.text.json ([a01b5ca](https://github.com/myarichuk/Json.Masker/commit/a01b5ca6e0a1a6eadd7f7bb0cab156563d92a825))
  
  
  
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



### Bug Fixes

- **ci:** now maybe publish pipeline would work (meh) ([a56694a](https://github.com/myarichuk/Json.Masker/commit/a56694ad63336cd1577fd8e6396ec3b6ce2f9129))
  
- **ci:** now properly fix changelog generation (stupid copy-paste error!) ([66003fe](https://github.com/myarichuk/Json.Masker/commit/66003febc144a30292c3f1fdb00e4aed9704494b))
  
  
  
  
  






## [v0.0.17.0](https://github.com/myarichuk/Json.Masker/compare/v0.0.16.0...v0.0.17.0) - 2025-09-24



### Bug Fixes

- make sure no irrelevant warnings during compilation ([620d17b](https://github.com/myarichuk/Json.Masker/commit/620d17bd7ec1705d038a99ff6151d91e97363b41))
  
  
  
  
  






## [v0.0.16.0](https://github.com/myarichuk/Json.Masker/compare/v0.0.14.0...v0.0.16.0) - 2025-09-24



  
  






## v0.0.14.0 - 2025-09-24



### Bug Fixes

- **ci:** finally adjust all missing gitversion variables ([2f22f1a](https://github.com/myarichuk/Json.Masker/commit/2f22f1a191fca01dd6542c36053fb0bdb4b6c3a6))
  
- **ci:** adjust gitversion action output variable name ([878403d](https://github.com/myarichuk/Json.Masker/commit/878403dae5dc2c73049c2ba1d9b2f0644abfdf7e))
  
- **ci:** proper config for gitversion ([e0b9845](https://github.com/myarichuk/Json.Masker/commit/e0b984512317bda2571cef353310ac2708f10298))
  
- **ci:** try to fix gitversion versioning issue ([8cdd8ed](https://github.com/myarichuk/Json.Masker/commit/8cdd8ed7c13d7e5e1a0664b528bdd20a78344415))
  
- **ci:** use GitVersion 6.x ([8dfb8c6](https://github.com/myarichuk/Json.Masker/commit/8dfb8c69738a7378cb77ab895e25cc514d165314))
  
  
  
### CI

- fix gitversion configuration ([6812bf6](https://github.com/myarichuk/Json.Masker/commit/6812bf6b7543f268dc89b81c4bbe3a306eef03e0))
  
- install dotnet 8 in workflows ([c5b7945](https://github.com/myarichuk/Json.Masker/commit/c5b7945cc0535e6b48d50996c9a9bbb50e1d5bb6))
  
  
  
  
  




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
  
  


