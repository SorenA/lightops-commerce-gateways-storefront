# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.4.0] - 2021-02-20

### Changed

- **Breaking** - Change handles to localized strings

## [0.3.1] - 2021-01-31

### Changed

- Fixed meta-field localization
- Filter non-public meta-fields out at service level

## [0.3.0] - 2021-01-31

### Added

- User context
- Languages and localization support based on header `Accept-Language`
- Currencies and multi-currency support based on header `X-Currency`
- Ability to serve supported languages through the GraphQL API
- Ability to serve supported currencies through the GraphQL API

### Changed

- **Breaking** - Migrated to .NET 5
- **Breaking** - Updated protobuf service and type definitions
- Use Protobuf generated models and services directly instead of mapping and re-implementing services, reduce code required by a lot
- **Breaking** - Change protobuf service namespace to prevent message clashes when using multiple services
- Upgrade to latest gRPC and GraphQL libraries
- Integrated GraphQL configuration into Storefront Gateway registration

### Removed

- **Breaking** - Local entity interfaces, models and mappers for products, no longer needed
- **Breaking** - Local entity interfaces, models and mappers for categories, no longer needed
- **Breaking** - Local entity interfaces, models and mappers for content pages, no longer needed
- **Breaking** - Local entity interfaces, models and mappers for meta-fields, no longer needed
- **Breaking** - Local entity interfaces, models and mappers for navigations, no longer needed

## [0.2.4] - 2020-12-28

### Added

- Image focal center

## [0.2.3] - 2020-09-23

### Changed

- Updated GraphQL libraries

## [0.2.2] - 2020-08-26

### Added

- Image CDN prefixing

## [0.2.1] - 2020-08-15

### Added

- Product and product variant primary images

## [0.2.0] - 2020-08-14

### Added

- Category models
- Product models
- Money models
- Image models
- Category gRPC service connection
- Product gRPC service connection
- Sample applications with mock data for categories and products
- Data loader for loading categories by id or handle in bulk
- Data loader for loading navigations by id or handle in bulk
- Data loader for loading content pages by id or handle in bulk
- Data loader for loading products by id or handle in bulk
- Data loader for loading meta-fields by id or parent id in bulk

### Changed

- **Breaking** - Updated refactored service definition for Navigation gRPC service
- **Breaking** - Updated refactored service definition for Content Page gRPC service
- **Breaking** - Updated refactored service definition for Meta-field gRPC service

## [0.1.0] - 2020-07-04

### Added

- CHANGELOG file
- README file describing project
- Azure Pipelines based CI/CD setup
- GraphQL API
- Navigation models
- Content Page models
- Meta-field models
- Navigation gRPC service connection
- Content Page gRPC service connection
- Meta-field gRPC service connection
- Sample applications with mock data and GraphQL API

[unreleased]: https://github.com/SorenA/lightops-commerce-gateways-storefront/compare/0.4.0...develop
[0.4.0]: https://github.com/SorenA/lightops-commerce-gateways-storefront/tree/0.4.0
[0.3.1]: https://github.com/SorenA/lightops-commerce-gateways-storefront/tree/0.3.1
[0.3.0]: https://github.com/SorenA/lightops-commerce-gateways-storefront/tree/0.3.0
[0.2.4]: https://github.com/SorenA/lightops-commerce-gateways-storefront/tree/0.2.4
[0.2.3]: https://github.com/SorenA/lightops-commerce-gateways-storefront/tree/0.2.3
[0.2.2]: https://github.com/SorenA/lightops-commerce-gateways-storefront/tree/0.2.2
[0.2.1]: https://github.com/SorenA/lightops-commerce-gateways-storefront/tree/0.2.1
[0.2.0]: https://github.com/SorenA/lightops-commerce-gateways-storefront/tree/0.2.0
[0.1.0]: https://github.com/SorenA/lightops-commerce-gateways-storefront/tree/0.1.0
