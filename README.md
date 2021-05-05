# Seventh Data headless CMS

Seventh Data headless CMS is an ASP.NET 5 C# base CMS management library.

# You can

- Manage article
- Manage channel, category, sub category
- Manage author

## Manage articles.

"Article" is the core data object of this service.  

You can 
- Create (Upsert)
- Search
- Fetch


## Manage channels, categories, sub categories. 

### Channel

"Channel" is the top-level group.  You can use GetChannels method with the owner Id to get a list of all channels owned by the owner.  

### Category

"Category" is the second-level group.  You can use GetCategoriesByChannelId or GetCategoriesByChannelPermaName, both method returns PaginationModel<ArticleCategoryViewModel>.  

Once you know which category object you want, then you can use GetCategoryByPermaName or GetCategoryByCategoryId.  Both methods returns ArticleCategoryViewModel object.   

If you want to include deleted categories, please use GetCategoryByPermaNameAdmin or GetCategoryByCategoryIdAdmin.  Both methods will return ArticleCategoryViewModel with deleted item in it.

### Sub Category
"SubCategory" is the third-level group.  You can use GetSubCategoriesByCategoryPermaName or GetSubCategoriesByCategoryId.  Both methods returns ArticleSubCategoryViewModel object.  


## Manage author.


# Related items